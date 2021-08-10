using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Request
{
    public class BetRequest
    {
        [Required]
        public int SlotId { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int BetTypeId { get; set; }

        public BetRequest(int slotId, double amount, int betTypeId)
        {
            SlotId = slotId;
            Amount = amount;
            BetTypeId = betTypeId;
        }
    }
}
