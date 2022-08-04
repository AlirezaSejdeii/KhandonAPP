using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Request.Auth
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string EncryptionKey { get; set; }
    }
}
