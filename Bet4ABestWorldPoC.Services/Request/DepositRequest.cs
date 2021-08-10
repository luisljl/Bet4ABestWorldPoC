using System.ComponentModel.DataAnnotations;

namespace Bet4ABestWorldPoC.Services.Request
{
    public class DepositRequest
    {
        [Required]
        public double Amount { get; set; }
        [Required]
        public int MerchantId { get; set; }

        public DepositRequest(double amount, int merchantId)
        {
            Amount = amount;
            MerchantId = merchantId;
        }

    }
}
