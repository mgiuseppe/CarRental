using CarRentalNovility.Entities;
using System;

namespace CarRentalNovility.Web.Dto
{
    public class ReservationDto
    {
        public long Id { get; set; }
        public CarDto Car { get; set; }
        public ClientDto Client { get; set; }

        public ReservationState State { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public ClientAccountDto ClientAccount { get; set; }
    }
}
