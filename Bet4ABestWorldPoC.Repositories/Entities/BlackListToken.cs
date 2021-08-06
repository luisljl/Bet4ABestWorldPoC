using System;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    public class BlackListToken
    {
        public DateTime CreatedOn { get; set; }
        public string InvalidToken { get; set; }
    }
}
