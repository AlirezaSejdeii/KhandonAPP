using Khandon.Shared.Dto.Base;
using Khandon.Shared.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Result
{
    public class StudyResult:StudyBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ChapterId { get; set; }

        public BookTypeEnum BookType { get; set; }

    }
}
