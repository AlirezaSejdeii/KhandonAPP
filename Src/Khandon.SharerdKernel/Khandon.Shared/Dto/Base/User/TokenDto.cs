using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request.Auth
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ExpireDateUtc { get; set; }
    }
}
