using BusinessLayer.Contracts;
using DataLayer.Repositories.Contracts;
using CarRentalNovility.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CarRentalNovility.DataLayer;

namespace CarRentalNovility.BusinessLayer
{
    public partial class BLReservation : IBLReservation
    {
        private readonly IRepositoryReservation reservationRep;
        private readonly IRepositoryCar carRep;
        private readonly IRepositoryClient clientRep;
        private readonly IUnitOfWork uow;

        public BLReservation(IRepositoryReservation reservationRep, IRepositoryCar carRep, IRepositoryClient clientRep, IUnitOfWork uow)
        {
            this.reservationRep = reservationRep;
            this.carRep = carRep;
            this.clientRep = clientRep;
            this.uow = uow;
        }

        public async Task<Reservation> BookCarReservationAsync(BookCarReservationParameters parameters)
        {
            var clientTask = clientRep.GetAsync(parameters.ClientId);
            var carTask = carRep.GetAsync(parameters.CarId);
            var client = await clientTask;
            var car = await carTask;

            await ValidateAsync(parameters, car, client);

            var reservation = new Reservation()
            {
                Car = car,
                Client = client,
                PickUpDateTime = parameters.PickUpDateTime,
                ReturnDateTime = parameters.ReturnDateTime,
                State = ReservationState.Booked
            };
            reservation.ClientAccount = new ClientAccount()
            {
                CancellationFeeValueAtBookingMoment = car.Type.CancellationRateFee,
                RentalRateFeeValueAtBookingMoment = car.Type.RentalRateFee,
                DepositFeePaid = CalculateDepositFeeToPay(reservation)
            };

            reservationRep.Insert(reservation);

            await uow.CompleteAsync();
            return reservation;
        }

        public async Task<Reservation> CancelCarReservationAsync(long reservationId)
        {
            var desiredState = ReservationState.Cancelled;
            var reservation = await reservationRep.GetAsync(reservationId);

            Validate(reservation, desiredState, reservationId);

            reservation.State = desiredState;
            reservation.ClientAccount.CancellationFeePaid = CalculateCancellationFeeToPay(reservation, DateTime.Now);
            reservationRep.Update(reservation);

            await uow.CompleteAsync();
            return reservation;
        }

        public async Task<Reservation> PickUpCarAsync(long reservationId)
        {
            var desiredState = ReservationState.PickedUp;
            var reservation = await reservationRep.GetAsync(reservationId);

            Validate(reservation, desiredState, reservationId);

            reservation.State = desiredState;
            reservationRep.Update(reservation);

            await uow.CompleteAsync();
            return reservation;
        }

        public async Task<Reservation> ReturnCarAsync(long reservationId)
        {
            var desiredState = ReservationState.Returned;
            var reservation = await reservationRep.GetAsync(reservationId);
            Validate(reservation, desiredState, reservationId);

            reservation.State = desiredState;
            reservation.ClientAccount.RentalFeePaid = CalculateRentalFeeToPay(reservation);
            reservationRep.Update(reservation);

            await uow.CompleteAsync();
            return reservation;
        }

        public async Task<List<Reservation>> BrowseReservationAsync(BrowseReservationsParameters parameters)
        {
            return await reservationRep.BrowseReservationAsync(parameters);
        }
    }
}
