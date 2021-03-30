using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tests.CommandAppServicesIntegrationTests;

namespace ApplicationServiceTests
{
    [TestClass]
    public class SetUp
    {
        [AssemblyInitialize()]
        public static async Task AssemblyInit(TestContext context)
        {
            var dbContextFactory = new SampleDbContextFactory();
            using (var dbContext = dbContextFactory.CreateDbContext(new string[] { }))
            {
                await dbContext.Database.EnsureCreatedAsync();

                dbContext.Database.BeginTransaction();

                SystemParameter monthlyIncomeLimit = new SystemParameter("MonthlyIncomeLimit", 1000000.00m);
                dbContext.SystemParameters.Add(monthlyIncomeLimit);
                dbContext.SaveChanges();

                SystemParameter monthlyOutcomeLimit = new SystemParameter("MonthlyOutcomeLimit", 1000000.00m);
                dbContext.SystemParameters.Add(monthlyOutcomeLimit);
                dbContext.SaveChanges();

                SystemParameter provisionLimit = new SystemParameter("ProvisionLimit", 10000.00m);
                dbContext.SystemParameters.Add(provisionLimit);
                dbContext.SaveChanges();

                SystemParameter provisionUnderLimitCost = new SystemParameter("ProvisionUnderLimitCost", 100.00m);
                dbContext.SystemParameters.Add(provisionUnderLimitCost);
                dbContext.SaveChanges();

                SystemParameter provisionOverLimitCostPercent = new SystemParameter("ProvisionOverLimitCostPercent", 1.00m);
                dbContext.SystemParameters.Add(provisionOverLimitCostPercent);
                dbContext.SaveChanges();

                dbContext.Database.CommitTransaction();
            }
        }

        [AssemblyCleanup()]
        public static async Task AssemblyCleanup()
        {
            var dbContextFactory = new SampleDbContextFactory();
            using (var dbContext = dbContextFactory.CreateDbContext(new string[] { }))
            {
                await dbContext.Database.EnsureDeletedAsync();
            }
        }
    }
}
