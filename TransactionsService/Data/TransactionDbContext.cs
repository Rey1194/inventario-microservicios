using Microsoft.EntityFrameworkCore;
using TransactionsService.Models;

namespace TransactionsService.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
