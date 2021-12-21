using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.Entities
{
    public class Loan
    {
        public int LoanId { get; set; }
        public DateTime Date { get; set; }

        [Precision(13, 2)]
        public decimal Amount { get; set; }
        public int Duration { get; set; }

        [Precision(13, 2)]
        public decimal Payments { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = null!;

        public Account Account { get; set; } = null!;
    }
}
