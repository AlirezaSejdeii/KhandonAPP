using Khandon.Shared.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request
{
    public class StudyEdit : StudyBase
    {
        public Guid Id { get; set; }
    }
}
