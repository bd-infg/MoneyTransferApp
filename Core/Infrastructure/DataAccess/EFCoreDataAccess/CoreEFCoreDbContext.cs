using Common.EFCoreDataAccess;
using Domain.Entities;
using EFCoreDataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDataAccess
{
    public class CoreEFCoreDbContext : EFCoreDbContext
    {

        public CoreEFCoreDbContext(DbContextOptions<CoreEFCoreDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new SystemParameterConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
