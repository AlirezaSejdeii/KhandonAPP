using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.User.Identity
{
    public interface IEmailService
    {
        bool SendMail(string Name, string Title, string Email, string Body);
    }
}
