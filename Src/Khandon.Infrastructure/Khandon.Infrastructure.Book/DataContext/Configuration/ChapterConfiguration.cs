using Khandon.Domain.Enitties.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Book.DataContext.Configuration
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Domain.Enitties.Book.Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.Title).IsRequired().HasMaxLength(150);
            builder.Property(a => a.ShortDescription).HasMaxLength(500);
        }
    }
}
