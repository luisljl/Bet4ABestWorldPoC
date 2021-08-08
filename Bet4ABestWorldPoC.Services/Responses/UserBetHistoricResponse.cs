using System.Collections.Generic;

namespace Bet4ABestWorldPoC.Services.Responses
{
    public class UserBetHistoricResponse
    {
        public string SlotName { get; set; }
        public List<BetHistoricResponse> Bets { get; set; }
    }
}
