using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Responses
{
    public class UserBetHistoryResponse
    {
        public string SlotName { get; set; }
        public List<BetHistoryResponse> Bets { get; set; }
    }
}
