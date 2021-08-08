namespace Bet4ABestWorldPoC.Services.Request
{
    public class DepositRequest
    {
        public double Amount { get; set; }
        public int MerchantId { get; set; }

        public DepositRequest(double amount, int merchantId)
        {
            Amount = amount;
            MerchantId = merchantId;
        }

    }
}
