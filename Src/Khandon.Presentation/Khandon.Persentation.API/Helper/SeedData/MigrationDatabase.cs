using Khandon.Infrastructure.Book.DataContext;
using Khandon.Infrastructure.Users.DataContext;

namespace Khandon.Persentation.API.Helper.SeedData
{
    public static class MigrationDatabase
    {
        public static void MigrationToDatabase(this WebApplicationBuilder builder)
        {
            BookContext bookContext = builder.Services.BuildServiceProvider().GetRequiredService<BookContext>();
            bookContext.Database.EnsureCreated();
            Console.WriteLine("book database was created");

            UserDbContext userDbContext = builder.Services.BuildServiceProvider().GetRequiredService<UserDbContext>();
            userDbContext.Database.EnsureCreated();
            Console.WriteLine("user database was created");

            Seed.SeedData(builder);

        }
    }
}
