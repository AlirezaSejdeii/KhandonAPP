using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request.User
{
    public class UserLogin
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
