using Khandon.Domain.Enitties.Book;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Book.DataContext
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }


        public DbSet<Domain.Enitties.Book.BookGroup> BookGroups { get; set; }
        public DbSet<Domain.Enitties.Book.Book> Books { get; set; }
        public DbSet<Domain.Enitties.Book.Chapter> Chapters { get; set; }
        public DbSet<Domain.Enitties.Book.Study> Studies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());            
        }
    }
}
