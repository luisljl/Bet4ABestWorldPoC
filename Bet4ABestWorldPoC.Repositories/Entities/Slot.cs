using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    [Table("Slot")]
    public class Slot
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RTP { get; set; }
    }
}
