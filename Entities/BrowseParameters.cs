using System;

namespace CarRentalNovility.Entities
{
    public class BrowseReservationsParameters
    {
        public ReservationState? ReservationState { get; set; }
        public DateTime? PickedUpDateTimeFrom { get; set; }
        public DateTime? PickedUpDateTimeTo { get; set; }
        public string ClientFullName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNumber { get; set; }
    }
}
