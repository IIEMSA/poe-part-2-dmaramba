using Microsoft.EntityFrameworkCore;
using PoeSample.Models.Seeders;

namespace PoeSample.Models
{
    public class ClaimsContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Person> People { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<ClaimDocument> ClaimDocuments { get; set; }

        public DbSet<ClaimStatus> ClaimStatuses { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //connection string
        //    //replace dabase name 
        //    //replace root user if you a custom username
        //    //replace RootPassword
        //    optionsBuilder.UseMySQL("server=localhost;database=YourDatabaseName;user=root;password=RootPassword");
        //    optionsBuilder.UseMySQL("server=mysql-11060b70-dumisani-feb1.f.aivencloud.com,18264;database=defaultdb;user=avnadmin;password=AVNS_-dn-0vFCnZBd08VqmXU");
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "DatabaseDB");
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
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
        }

    }
}
