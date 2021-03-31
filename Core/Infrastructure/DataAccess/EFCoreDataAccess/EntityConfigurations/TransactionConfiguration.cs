using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDataAccess.EntityConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> modelBuilder)
        {
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Property(a => a.Amount).HasPrecision(12, 2);
            modelBuilder.Property(a => a.FromAccountId).HasMaxLength(13);
            modelBuilder.Property(a => a.ToAccountId).HasMaxLength(13);
            modelBuilder.Property(a => a.DateTime).HasColumnType("datetime");
        }
    }
}
