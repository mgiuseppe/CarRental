using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalNovility.Entities
{
    public class BookCarReservationParameters
    {
        public long CarId { get; set; }
        public long ClientId { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
    }
}
