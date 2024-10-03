using Microsoft.EntityFrameworkCore;

namespace PoeSample.Models
{
    public class ClaimsContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Person> People { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<ClaimStatus> ClaimStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //connection string
            optionsBuilder.UseMySQL("server=localhost;database=Claims;user=user;password=password");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasKey(x => x.Id);

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(p => p.Person).WithMany(x => x.Claims);
            });
        }

    }
}
