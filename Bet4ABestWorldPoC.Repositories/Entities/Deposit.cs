using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    [Table("Deposit")]
    public class Deposit
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public double Amount { get; set; }
        public int MerchantId { get; set; }
    }
}
