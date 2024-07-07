using Microsoft.EntityFrameworkCore;
using UserOrgs.Configurations;

namespace UserOrgs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<UserOrganisation> UserOrganisations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new UserConfig());
            modelBuilder.ApplyConfiguration<Organisation>(new OrganisationConfig());
            modelBuilder.ApplyConfiguration<UserOrganisation>(new UserOrganisationConfig());
        }
    }
}
