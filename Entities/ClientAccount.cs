namespace CarRentalNovility.Entities
{
    public class ClientAccount
    {
        public long Id { get; set; }

        public decimal CancellationFeeValueAtBookingMoment { get; set; }
        public decimal RentalRateFeeValueAtBookingMoment { get; set; }

        public decimal DepositFeePaid { get; set; }
        public decimal CancellationFeePaid { get; set; }
        public decimal RentalFeePaid { get; set; }
    }
}
