using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Base.User
{
    public class ConfirmationModelDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
