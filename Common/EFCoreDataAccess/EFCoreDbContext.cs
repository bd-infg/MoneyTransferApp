using Microsoft.EntityFrameworkCore;

namespace Common.EFCoreDataAccess
{
    public class EFCoreDbContext : DbContext
    {
        public EFCoreDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
