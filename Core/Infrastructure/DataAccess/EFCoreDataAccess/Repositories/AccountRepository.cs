using Common.EFCoreDataAccess;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDataAccess.Repositories
{
    public class AccountRepository : EFCoreRepository<Account>, IAccountRepository
    {
        public AccountRepository(CoreEFCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
