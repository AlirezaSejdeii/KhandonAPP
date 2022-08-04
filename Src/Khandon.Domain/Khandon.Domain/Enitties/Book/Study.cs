using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Domain.Enitties.Book
{
    public class Study:BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public int Length { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }

        public int Flag { get; set; }

        //Navigation

        public Chapter Chapter { get; set; }
        public Guid ChapterId { get; set; }
    }
}
