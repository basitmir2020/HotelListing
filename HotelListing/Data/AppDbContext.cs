using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions context):base(context)
        { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData
                (
                    new Country
                    {
                        Id = 1,
                        Name = "India",
                        ShortName = "IND"
                    },
                    new Country
                    {
                        Id = 2,
                        Name = "Bahamas",
                        ShortName = "BS"
                    },
                    new Country
                    {
                        Id = 3,
                        Name = "United States Of America",
                        ShortName = "USA"
                    }
                );

            modelBuilder.Entity<Hotel>().HasData
                (
                    new Hotel
                    {
                        Id = 1,
                        Name = "Lalit",
                        Address = "Srinagar",
                        Ratting = 4.5,
                        CountryId = 1,
                    },
                    new Hotel
                    {
                        Id = 2,
                        Name = "Grand Palldium",
                        Address = "Nassua",
                        Ratting = 5.4,
                        CountryId = 2,
                    },
                    new Hotel
                    {
                        Id = 3,
                        Name = "Wyndham ATT",
                        Address = "Texas",
                        Ratting = 2.7,
                        CountryId = 3,
                    }
                );
        }
    }
}
