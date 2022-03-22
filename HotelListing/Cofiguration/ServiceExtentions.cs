using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelListing.Cofiguration
{
    public static class ServiceExtentions
    {
        public static void ConfigureIdentity(this IServiceCollection service)
        {
            var builder = service
                .AddIdentityCore<ApiUser>(
                q =>q.User.RequireUniqueEmail = true);
            builder = new IdentityBuilder(builder.UserType, 
                typeof(IdentityRole), service);
            builder.AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        /*public static void ConfigureJWT(this IServiceCollection service,IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var key = jwtSettings.Key;
            service.AddAuthentication(options =>
            {
                *//*options.DefaultAuthenticateScheme = JwtBearerDefaults*//*
            });
        }*/
    }
}
