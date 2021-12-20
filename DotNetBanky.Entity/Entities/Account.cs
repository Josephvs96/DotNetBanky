namespace DotNetBanky.Entity.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; } = null!;
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }

        public ICollection<Disposition> Dispositions { get; set; } = new List<Disposition>();
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<PermenentOrder> PermenentOrders { get; set; } = new List<PermenentOrder>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
