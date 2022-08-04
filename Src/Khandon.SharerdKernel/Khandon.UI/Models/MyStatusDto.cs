using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Models
{
    public class MyStatusDto
    {
        public bool IsData { get; set; }
        public int DificaltyAvg { get; set; }
        public int MaxReadingPage { get; set; }
        public int MaxReadingMinute { get; set; }

        public int AvgReadingPage { get; set; }
        public int AvgReadingMinute { get; set; }

        public int TotalReadingPage { get; set; }
        public int TotalReadingMinute { get; set; }

        public IDictionary<string,int> ActiveStudyGroup { get; set; }

       //public IDictionary<string,int> ActiveStudyReads { get; set; }

    }
}
