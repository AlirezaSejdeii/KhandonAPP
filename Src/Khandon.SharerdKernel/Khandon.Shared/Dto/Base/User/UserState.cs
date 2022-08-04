using Khandon.Shared.Dto.Request.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Base.User
{
    public class UserState
    {
        public TokenDto tokenDto { get; set; }
        public string Username { get; set; }
    }
}
