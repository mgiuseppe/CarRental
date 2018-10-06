using BusinessLayer.Contracts;
using CarRentalNovility.Entities;
using CarRentalNovility.Entities.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalNovility.BusinessLayer
{
    public partial class BLReservation : IBLReservation
    {
        #region calculation helpers

        private decimal CalculateDepositFeeToPay(Reservation reservation)
        {
            return reservation.DurationInHours * reservation.Car.Type.RentalRateFee * reservation.Car.Type.DepositFee;
        }

        /// <summary>
        /// Cancellation Fee is the price of one or more hours, but not more than total amount of reservation.
        /// </summary>
        /// <returns></returns>
        private decimal CalculateCancellationFeeToPay(Reservation reservation, DateTime cancellationTime)
        {
            var totalAmountOfReservation = CalculateRentalFeeToPay(reservation);
            var cancellationFeePerHour = reservation.ClientAccount.CancellationFeeValueAtBookingMoment;
            var hoursElapsedSincePickUp = (decimal)(cancellationTime - reservation.PickUpDateTime).TotalHours;

            var cancellationFeeToPay = Math.Min(totalAmountOfReservation, cancellationFeePerHour * hoursElapsedSincePickUp);

            return cancellationFeeToPay > 0 ? cancellationFeeToPay : cancellationFeePerHour;
        }

        private decimal CalculateRentalFeeToPay(Reservation reservation)
        {
            return reservation.DurationInHours * reservation.ClientAccount.RentalRateFeeValueAtBookingMoment - reservation.ClientAccount.DepositFeePaid;
        }

        #endregion

        #region validation helpers

        private async Task ValidateAsync(BookCarReservationParameters parameters, Car car, Client client)
        {
            if (car == null)
                throw new CustomException($"Unknown Car - Id {parameters.CarId}", ErrorCode.UnknownCar);
            if (client == null)
                throw new CustomException($"Unknown Client - Id {parameters.ClientId}", ErrorCode.UnknownClient);
            if (parameters.PickUpDateTime < DateTime.Now)
                throw new CustomException($"PickUpDateTime must be greater than current DateTime", ErrorCode.PickUpDateTimeInThePast);
            if (parameters.PickUpDateTime >= parameters.ReturnDateTime)
                throw new CustomException($"ReturnDateTime must be greater than PickUpDateTime", ErrorCode.ReturnDateTimeBeforePickUpDateTime);

            var pendingReservations = await reservationRep.GetPendingReservationByCarAsync(car.Id, parameters.PickUpDateTime, parameters.ReturnDateTime);
            if (pendingReservations.Any())
                throw new CustomException($"Car is not available in the selected period - CarId {car.Id} - PickUpDateTime {parameters.PickUpDateTime} - ReturnDateTime {parameters.ReturnDateTime} - Existing pending reservation id {pendingReservations.First().Id}", ErrorCode.PendingReservationExists);
        }

        private void Validate(Reservation reservation, ReservationState desiredState, long desiredReservationId)
        {
            if (reservation == null)
                throw new CustomException($"Unknown Reservation - Id {desiredReservationId}", ErrorCode.UnknownReservation);
            if (!ValidateDesiredState(reservation, desiredState))
                throw new CustomException($"Invalid state change - {reservation.State} -> {desiredState}", ErrorCode.InvalidStateTransition);
        }

        private bool ValidateDesiredState(Reservation reservation, ReservationState desiredState)
        {
            switch (desiredState)
            {
                case ReservationState.Cancelled:
                case ReservationState.PickedUp:
                    return reservation.State == ReservationState.Booked;
                case ReservationState.Returned:
                    return reservation.State == ReservationState.PickedUp;
                default:
                    throw new CustomException($"Unknown State {desiredState}", ErrorCode.UnknownDesiredState);
            }
        }

        #endregion
    }
}
