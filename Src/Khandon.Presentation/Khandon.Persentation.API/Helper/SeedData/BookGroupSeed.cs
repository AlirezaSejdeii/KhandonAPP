using Khandon.Infrastructure.Book.DataContext;

namespace Khandon.Persentation.API.Helper.SeedData
{
    public class BookGroupSeed
    {
        public BookGroupSeed(BookContext bookContext)
        {
            if (!bookContext.BookGroups.Any())
            {
                bookContext.BookGroups.Add(new() {Title="روانشناسی", ShortDescription = "روانشناسی و انواع مربوط به آن" });
                bookContext.BookGroups.Add(new() {Title="برنامه نویسی", ShortDescription = "مطالب مربوط به برنامه نویسی" });
                bookContext.BookGroups.Add(new() { Title="فلسفه", ShortDescription = "فلسفه و شبه علم و کلیات" });
                bookContext.SaveChanges();
            }
        }
    }
}
