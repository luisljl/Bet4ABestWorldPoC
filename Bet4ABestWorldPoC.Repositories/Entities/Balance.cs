using System;

namespace Bet4ABestWorldPoC.Repositories.Entities
{
    public class Balance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
