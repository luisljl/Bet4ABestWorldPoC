using System;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    public class Bet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public int BetType { get; set; }
        public bool WinningBet { get; set; }
        public double Amount { get; set; }
        public double WinningAmount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
