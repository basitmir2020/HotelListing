using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Cofiguration.Entity
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
