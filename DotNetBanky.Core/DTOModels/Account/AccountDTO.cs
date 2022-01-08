using DotNetBanky.Core.DTOModels.Transaction;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.DTOModels.Account
{
    public class AccountDetailsDTO
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; } = null!;
        public DateTime Created { get; set; }
        [Precision(13, 2)]
        public decimal Balance { get; set; }

        public ICollection<TransactionDTO> Transactions { get; set; } = null!;
    }

    public class AccountSummeryDTO
    {
        public int AccountId { get; set; }
        public string Frequency { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public decimal Balance { get; set; }
    }

    public class AccountCreateModel
    {
        [Required]
        public string Frequency { get; set; } = null!;
        [Required]
        public string AccountType { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [Precision(13, 2)]
        public decimal Balance { get; set; } = 0;
        public int CustomerId { get; set; }
    }
}
