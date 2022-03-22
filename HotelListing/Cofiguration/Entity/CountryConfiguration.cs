using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Cofiguration.Entity
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
                    });
        }
    }
}
