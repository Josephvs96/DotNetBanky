using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DotNetBanky.Core.DTOModels.Transaction
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }

        public string Type { get; set; } = null!;

        public string Operation { get; set; } = null!;

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public string? Symbol { get; set; }

        public string? Bank { get; set; }

        public string? Account { get; set; }
    }

    public class TransactionCreateDTO
    {
        public int CustomerId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string Type { get; set; } = null!;

        public string Operation { get; set; } = null!;

        [Precision(14, 2)]
        public decimal Amount { get; set; }

        public string? Symbol { get; set; }

        [DisplayName("Account From")]
        public int AccountFrom { get; set; }

        [DisplayName("Account To")]
        public int? AccountTo { get; set; }
    }
}
