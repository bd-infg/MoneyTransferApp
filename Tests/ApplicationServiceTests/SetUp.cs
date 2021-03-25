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
