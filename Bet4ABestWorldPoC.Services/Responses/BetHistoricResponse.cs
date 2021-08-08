using System;

namespace Bet4ABestWorldPoC.Services.Responses
{
    public class BetHistoricResponse
    {
        public int Id { get; set; }
        public double BetAmount { get; set; }
        public int SlotId { get; set; }
        public int BetType { get; set; }
        public bool WinningBet { get; set; }
        public double WinningAmount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
