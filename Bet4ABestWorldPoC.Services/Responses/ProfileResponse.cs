using System.Collections.Generic;

namespace Bet4ABestWorldPoC.Services.Responses
{
    public class ProfileResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<UserBetHistoricResponse> BetHistoric { get; set; } = new List<UserBetHistoricResponse>();
        public List<DepositHistoricResponse> DepositHistoric { get; set; } = new List<DepositHistoricResponse>();
        public BalanceResponse CurrentBalance { get; set; }
    }
}
