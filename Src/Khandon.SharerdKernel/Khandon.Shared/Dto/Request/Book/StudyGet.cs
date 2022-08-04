using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request
{
    public class StudyGet
    {
        public int Page { set; get; } =1;
        public int Limit { get; set; } = 100;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int BookGroupId { get; set; } = 0;

    }
}
