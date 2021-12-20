namespace DotNetBanky.Entity.Entities
{
    public class PermenentOrder
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public string BankTo { get; set; } = null!;
        public string AccountTo { get; set; } = null!;
        public decimal? Amount { get; set; }
        public string Symbol { get; set; } = null!;

        public Account Account { get; set; } = null!;
    }
}
