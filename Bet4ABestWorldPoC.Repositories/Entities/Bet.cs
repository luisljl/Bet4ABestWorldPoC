using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    [Table("Bet")]
    public class Bet
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Slot")]
        public int SlotId { get; set; }
        public int BetType { get; set; }
        public bool WinningBet { get; set; }
        public double Amount { get; set; }
        public double WinningAmount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
