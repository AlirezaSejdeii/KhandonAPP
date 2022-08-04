using Khandon.Infrastructure.Book.DataContext;
using Khandon.Infrastructure.Users.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Khandon.Persentation.API.Helper.SeedData
{
    public static class Seed
    {
        public static void SeedData(this WebApplicationBuilder builder)
        {
            BookContext bookContext= builder.Services.BuildServiceProvider().GetRequiredService<BookContext>();
            RoleManager<ApplicationRole> roleManager= builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<ApplicationRole>>();

            BookGroupSeed bookGroupSeed=new BookGroupSeed(bookContext);
            Console.WriteLine("Bookgroup data was seeded");

            RolesSeed rolesSeed=new RolesSeed(roleManager);
            Console.WriteLine("Roles was seeded");
        }
    }
}
