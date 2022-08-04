using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request.User
{
    public class RecoveryPasswordConfirmViewModel
    {
        public string EmailOrUsername { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
