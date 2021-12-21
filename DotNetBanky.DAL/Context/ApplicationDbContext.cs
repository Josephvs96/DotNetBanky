using DotNetBanky.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Disposition> Dispositions { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;
        public DbSet<PermenentOrder> PermenentOrders { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
    }
}
