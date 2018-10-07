using System;
using System.Collections.Generic;
using System.Net;

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

    public static class ErrorCodeExtensions
    {
        static readonly Dictionary<ErrorCode, HttpStatusCode> mapping = new Dictionary<ErrorCode, HttpStatusCode>
        {
            {ErrorCode.GenericException,                       HttpStatusCode.InternalServerError               },
            {ErrorCode.UnknownClient,                          HttpStatusCode.NotFound                          },
            {ErrorCode.UnknownCar,                             HttpStatusCode.NotFound                          },
            {ErrorCode.PickUpDateTimeInThePast,                HttpStatusCode.BadRequest                        },
            {ErrorCode.ReturnDateTimeBeforePickUpDateTime,     HttpStatusCode.BadRequest                        },
            {ErrorCode.PendingReservationExists,               HttpStatusCode.BadRequest                        },
            {ErrorCode.UnknownReservation,                     HttpStatusCode.NotFound                          },
            {ErrorCode.InvalidStateTransition,                 HttpStatusCode.BadRequest                        },
            {ErrorCode.UnknownDesiredState,                    HttpStatusCode.NotFound}
        };

        public static HttpStatusCode ToHttpStatusCode(this ErrorCode internalErrorCode)
        {
            return mapping.TryGetValue(internalErrorCode, out HttpStatusCode httpStatusCodeValue)
                ? httpStatusCodeValue
                : HttpStatusCode.InternalServerError;
        }
    }
}
