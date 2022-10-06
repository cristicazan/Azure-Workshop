using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Azure_Workshop
{
    public class BankDbContext : DbContext
    {
        public DbSet<TransactionDao> Transactions { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
        {
        }
    }

    public class TransactionDao
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public int Value { get; set; }

        public string? Name { get; set; }
    }
}
