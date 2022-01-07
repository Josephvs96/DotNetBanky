using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetBanky.Core.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string Type { get; set; } = null!;

        [MaxLength(50)]
        public string Operation { get; set; } = null!;

        [Precision(13, 2)]
        public decimal Amount { get; set; }

        [Precision(13, 2)]
        public decimal Balance { get; set; }

        [MaxLength(50)]
        public string? Symbol { get; set; }

        [MaxLength(50)]
        public string? Bank { get; set; }

        [MaxLength(50)]
        public string? Account { get; set; }

        [ForeignKey("AccountId")]
        public Account AccountNavigation { get; set; } = null!;
    }
}
