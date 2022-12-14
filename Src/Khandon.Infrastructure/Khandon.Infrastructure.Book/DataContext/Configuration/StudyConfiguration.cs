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
    public class StudyConfiguration : IEntityTypeConfiguration<Domain.Enitties.Book.Study>
    {
        public void Configure(EntityTypeBuilder<Study> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.Title).IsRequired().HasMaxLength(100);
            builder.Property(a => a.ShortDescription).HasMaxLength(500);
            builder.Property(a => a.CreateDate).IsRequired();
            builder.Property(a => a.Length).IsRequired();
        }
    }
}
