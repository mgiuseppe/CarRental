namespace CarRentalNovility.Web.Dto
{
    public class CarTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Price per hour
        /// </summary>
        public decimal RentalRateFee { get; set; }

        public decimal CancellationRateFee { get; set; }

        /// <summary>
        /// Percentage of the rental rate 
        /// Range: [0..1]
        /// </summary>
        public decimal DepositFee { get; set; }
    }
}