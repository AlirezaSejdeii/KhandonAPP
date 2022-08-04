using Khandon.Core.Interfaces.User.Identity;
using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Query.User
{
    public class UserLoginQuery:IRequest<AuthResult>
    {
        public UserLogin userLogin { get; set; }
    }

    public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, AuthResult>
    {
        private readonly IIdentityService identityService;

        public UserLoginQueryHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<AuthResult> Handle(UserLoginQuery request, CancellationToken cancellationToken)
        {
            var result = await identityService.UserLoginAsync(request.userLogin);
            return result;
        }
    }
}
