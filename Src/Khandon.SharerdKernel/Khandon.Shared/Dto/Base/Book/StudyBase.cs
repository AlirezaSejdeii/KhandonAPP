using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Base
{
    public class StudyBase
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public int Length { get; set; }

        public int Flag { get; set; }

        //Navigation
    }
}
