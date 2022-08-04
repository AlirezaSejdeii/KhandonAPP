using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Response
{
    public class SocietyStatusDto
    {
        public int UserRank { get; set; }
        public int AllUsersCount { get; set; }
        public int SumStudiesPage { get; set; }
        public int SumStudiesMinut { get; set; }
        public int AvgStudyPerDayPage { get; set; }
        public int AvgStudyPerDayMinute { get; set; }
        public int AvgDificalty { get; set; }
        public IDictionary<string, int> ActiveStudyGroup { get; set; }
    }
}
