using Common.EFCoreDataAccess;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDataAccess.Repositories
{
    public class SystemParameterRepository : EFCoreRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository(CoreEFCoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
