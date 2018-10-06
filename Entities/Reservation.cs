using CarRentalNovility.Entities;
using System;

namespace CarRentalNovility.Entities
{
    public class Reservation
    {
        public long Id { get; set; }
        public Car Car { get; set; }
        public Client Client { get; set; }

        public ReservationState State { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public ClientAccount ClientAccount { get; set; }

        public decimal DurationInHours => (decimal)(ReturnDateTime - PickUpDateTime).TotalHours;
    }
}
