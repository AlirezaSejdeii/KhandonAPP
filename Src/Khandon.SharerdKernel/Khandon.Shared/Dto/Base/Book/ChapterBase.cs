using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Base
{
    public class ChapterBase
    {
        public string Title { get; set; }

        public string? ShortDescription { get; set; }

        //Navigation

        public Guid BookId { get; set; }
    }
}
