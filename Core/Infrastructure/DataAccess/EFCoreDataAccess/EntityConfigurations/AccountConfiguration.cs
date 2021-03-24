using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDataAccess.EntityConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> modelBuilder)
        {
            modelBuilder.Property(a => a.Id).ValueGeneratedNever().HasMaxLength(13);
            modelBuilder.Property(a => a.Balance).HasPrecision(12, 2);
            modelBuilder.Property(a => a.RowVersion).IsRowVersion();
            modelBuilder.Property(p => p.FirstName).HasMaxLength(30);
            modelBuilder.Property(p => p.LastName).HasMaxLength(30);
            modelBuilder.Property(p => p.Pin).HasMaxLength(4);
            modelBuilder.Property(p => p.Password).HasMaxLength(6);
            modelBuilder.Property(p => p.AccountNumber).HasMaxLength(18);
            modelBuilder.Property(p => p.LastTransactionDate).HasColumnType("datetime");
            modelBuilder.Property(p => p.OpeningDate).HasColumnType("datetime");
            modelBuilder.Property(a => a.MonthlyIncome).HasPrecision(12, 2);
            modelBuilder.Property(a => a.MonthlyOutcome).HasPrecision(12, 2);
        }
    }
}
