using EFCoreDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tests.CommandAppServicesIntegrationTests
{
    public class SampleDbContextFactory : IDesignTimeDbContextFactory<CoreEFCoreDbContext>
    {
        public CoreEFCoreDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<CoreEFCoreDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=testDB;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

            return new CoreEFCoreDbContext(options);
        }
    }

}
