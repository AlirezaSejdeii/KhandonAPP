using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request
{
    public class GetChapterByBookId
    {
        public Guid BookId { get; set; }
        public int limit { get; set; } = 10;
        public int page { get; set; } = 1;
    }
}
