using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.Entities
{
    public class PermenentOrder
    {
        [Key]
        public int OrderId { get; set; }

        [MaxLength(50)]
        public string BankTo { get; set; } = null!;

        [MaxLength(50)]
        public string AccountTo { get; set; } = null!;

        [Precision(13, 2)]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        public string Symbol { get; set; } = null!;

        public Account Account { get; set; } = null!;
    }
}
