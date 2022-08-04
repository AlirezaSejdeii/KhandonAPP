using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.User.Identity
{
    public interface IIdentityService
    {
        Task<AuthResult> ConfirmEmailAsync(string token, string email);
        Task<AuthResult> RecoveryPasswordAsync(string usernameOrEmail);
        Task<AuthResult> RecoveryPasswordConfirmAsync(RecoveryPasswordConfirmViewModel recovery);
        Task<AuthResult> RestPasswordAsync(RestPasswordViewModel restPassword, string userId);
        Task<AuthResult> UserLoginAsync(UserLogin userLogin);
        Task<AuthResult> UserSignUpAsync(UserSignUp userSignUp);
    }
}
