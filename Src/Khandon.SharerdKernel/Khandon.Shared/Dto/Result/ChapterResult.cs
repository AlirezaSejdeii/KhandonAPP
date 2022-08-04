using Khandon.Shared.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Result
{
    public class ChapterResult : ChapterBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }

        public List<StudyResult> Studies { get; set; }
    }
}
