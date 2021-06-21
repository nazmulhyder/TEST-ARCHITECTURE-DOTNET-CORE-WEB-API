using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Configuration.Entities;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //seeding country
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Bangladesh",
                    ShortName ="BD"
                });
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 2,
                    Name = "United Stated of America",
                    ShortName = "USA"
                });
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 3,
                    Name = "United Kingdom",
                    ShortName = "UK"
                });

            //seeding hotel
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sultans Dines",
                    Address = "Jigatola,Dhanmondi",
                    CountryId = 1,
                    Rating = 4.1
                });
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 2,
                    Name = "Start Kabab",
                    Address = "Green Road,Dhanmondi",
                    CountryId = 1,
                    Rating = 3.9
                });
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 3,
                    Name = "Kacchi Vhai",
                    Address = "Dhanmondi",
                    CountryId = 1,
                    Rating = 4.0
                });
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
    }
}
