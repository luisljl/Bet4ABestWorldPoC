using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Request
{
    public class BetRequest
    {
        public int SlotId { get; set; }
        public double Amount { get; set; }
        public int BetTypeId { get; set; }

        public BetRequest(int slotId, double amount, int betTypeId)
        {
            SlotId = slotId;
            Amount = amount;
            BetTypeId = betTypeId;
        }
    }
}
