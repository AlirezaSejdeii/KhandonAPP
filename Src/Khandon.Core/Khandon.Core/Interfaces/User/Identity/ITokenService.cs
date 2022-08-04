using Khandon.Infrastructure.Users.Models.User;
using Khandon.Shared.Dto.Request.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.User.Identity
{
    public interface ITokenService
    {
        TokenDto GenrateToken(ApplicationUser user);
    }
}
