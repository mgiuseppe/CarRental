using CarRentalNovility.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalNovility.Web.Dto
{
    public class BrowseReservationsParametersDto
    {
        public ReservationState? ReservationState { get; set; }
        public DateTime? PickedUpDateTimeFrom { get; set; }
        public DateTime? PickedUpDateTimeTo { get; set; }
        public string ClientFullName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNumber { get; set; }
    }
}
