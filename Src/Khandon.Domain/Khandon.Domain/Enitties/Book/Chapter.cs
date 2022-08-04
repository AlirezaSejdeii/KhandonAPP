using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Domain.Enitties.Book
{
    public class Chapter:BaseEntity<Guid>
    {
        public string Title { get; set; }

        public string? ShortDescription { get; set; }

        public bool IsDeleted { get; set; }


        //Navigation
        public List<Study> Studies { get; set; }

        public Book Book { get; set; }
        public Guid BookId { get; set; }
    }
}
