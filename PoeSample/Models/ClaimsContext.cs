﻿using Microsoft.EntityFrameworkCore;
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

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "DatabaseDB");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasKey(x => x.Id);
            modelBuilder.ApplyConfiguration(new ClaimConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new RateConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
        }

    }
}
