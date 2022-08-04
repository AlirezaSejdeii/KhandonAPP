using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Book.DataContext.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Domain.Enitties.Book.Book>
    {
        public void Configure(EntityTypeBuilder<Domain.Enitties.Book.Book> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.Title).IsRequired().HasMaxLength(150);
            builder.Property(a => a.UserId).IsRequired().HasMaxLength(500);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.CreateDate).IsRequired();
            builder.Property(a => a.Difficultye).IsRequired().HasMaxLength(10).HasDefaultValue(1);
            builder.Property(a => a.BookType).IsRequired();
            builder.Property(a => a.BookGroupId).IsRequired();

        }
    }
}
