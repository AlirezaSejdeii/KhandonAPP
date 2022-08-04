using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Base.User
{
    public class EmailConfig
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string SMTP_Address { get; set; }
        public int SMTP_Port { get; set; }
        public bool SSL { get; set; }
    }
}
