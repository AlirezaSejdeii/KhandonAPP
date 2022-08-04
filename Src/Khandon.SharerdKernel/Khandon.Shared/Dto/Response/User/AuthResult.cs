using Khandon.Shared.Dto.Request.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Response.User
{
    public class AuthResult
    {
        public TokenDto Token { get; set; }
        public bool Result { get; set; }
        public List<string> Messages { get; set; }
    }
}
