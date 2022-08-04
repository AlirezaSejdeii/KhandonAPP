using Khandon.Core.Interfaces.IRepository;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Infrastructure.Book.DataRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khandon.Infrastructure.Book
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBookInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookContext>(optoin =>
            {
                optoin.UseSqlServer(configuration.GetConnectionString("BookDb"));
                optoin.EnableSensitiveDataLogging();
            });

            services.AddTransient<IBookGroupRepository, BookGroupRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IChapterRepository, ChapterRepository>();
            services.AddTransient<IStudyRepository, StudyRepository>();
            return services;
        }
    }
}