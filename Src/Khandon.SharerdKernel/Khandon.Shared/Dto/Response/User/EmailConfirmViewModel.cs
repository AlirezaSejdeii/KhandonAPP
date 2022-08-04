using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request.Auth
{
    public class EmailConfirmViewModel
    {
        public string email { get; set; }
        public string token { get; set; }
    }
}
