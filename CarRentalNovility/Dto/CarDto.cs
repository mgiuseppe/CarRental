namespace CarRentalNovility.Web.Dto
{
    public class CarDto
    {
        public long Id { get; set; }
        public CarTypeDto Type { get; set; }
        public string PlateNumber { get; set; }
    }
}