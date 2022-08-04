using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Domain.Enitties.Book
{
    public class BookGroup:BaseEntity<int>
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
    }
}
