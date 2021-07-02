using Microsoft.EntityFrameworkCore;
using SoftAir.Data.Domain.Aircraft;

namespace SoftAir.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Aircraft> Aircraft { get; set; }

    }
}
