using Khandon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Domain.Enitties.Book
{
    public class Book:BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsComplited { get; set; }
        public byte Difficultye { get; set; }
        public BookType BookType { get; set; }
        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        //Navigations
        public BookGroup BookGroup { get; set; }
        public int BookGroupId { get; set; }

        public List<Chapter> Chapters { get; set; }

    }
}
