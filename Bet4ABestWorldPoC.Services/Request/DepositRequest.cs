using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Request
{
    public class DepositRequest
    {
        public DepositRequest(double amount, int merchantId)
        {
            Amount = amount;
            MerchantId = merchantId;
        }

        public double Amount { get; set; }
        public int MerchantId { get; set; }

    }
}
