using Common.EFCoreDataAccess;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDataAccess.Repositories
{
    public class TransactionRepository : EFCoreRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(CoreEFCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
