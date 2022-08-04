using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.CompositionDtos
{
    public class UserRankRemainingCount
    {
        public Guid BookId { get; set; }
        public int StudyCount { get; set; }
        public string UserId { get; set; }
    }
}
