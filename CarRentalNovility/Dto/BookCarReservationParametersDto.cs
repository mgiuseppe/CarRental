using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalNovility.Web.Dto
{
    public class BookCarReservationParametersDto
    {
        public long CarId { get; set; }
        public long ClientId { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
    }
}
