using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    [Table("BlackListToken")]
    public class BlackListToken
    {
        [Key]
        public string InvalidToken { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
