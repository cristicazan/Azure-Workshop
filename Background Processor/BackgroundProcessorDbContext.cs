using Microsoft.EntityFrameworkCore;
using System;

namespace Background_Processor
{
    public class BackgroundProcessorDbContext : DbContext
    {
        public DbSet<TransactionDao> Transactions { get; set; }

        public BackgroundProcessorDbContext(DbContextOptions<BackgroundProcessorDbContext> options)
        : base(options)
        {
        }

        //public BackgroundProcessorDbContext()
        //{

        //}
    }

    public class TransactionDao
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public int Value { get; set; }

        public string? Name { get; set; }
    }
}
