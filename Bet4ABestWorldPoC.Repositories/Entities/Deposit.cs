namespace Bet4ABestWorldPoC.Repositories.Entities
{
    public class Deposit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public int MerchantId { get; set; }
    }
}
