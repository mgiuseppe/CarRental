using System;

namespace CarRentalNovility.Entities.Exceptions
{
    public class CustomException : Exception
    {
        public ErrorCode code;

        public CustomException(string message, ErrorCode code) : base(message)
        {
            this.code = code;
        }
    }

    public enum ErrorCode
    {
        GenericException,
        UnknownClient,
        UnknownCar,
        PickUpDateTimeInThePast,
        ReturnDateTimeBeforePickUpDateTime,
        PendingReservationExists,
        UnknownReservation,
        InvalidStateTransition,
        UnknownDesiredState,
    }
}
