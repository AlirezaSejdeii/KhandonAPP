using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Domain
{
    public class BaseEntity<IId>
    {
        public IId Id { get; set; }
    }
}
