using Khandon.Shared.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.CompositionDtos
{
    public class StudyCompositionForGetPeriodReadTime
    {
        public BookTypeEnum BookType { get; set; }
        public DateTime CreateDate { get; set; }
        public int Length { get; set; }
    }
}
