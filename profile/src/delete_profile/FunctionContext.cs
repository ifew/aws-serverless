using Microsoft.EntityFrameworkCore;

namespace delete_profile
{
    public class FunctionContext : DbContext
    {
        public FunctionContext(DbContextOptions<FunctionContext> dbContextOptions) : base(dbContextOptions){ }

        public DbSet<ProfileModel> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}