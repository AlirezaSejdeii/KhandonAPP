using Khandon.Core.Interfaces.User.Identity;
using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using Khandon.Shared.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Command.Usere
{
    public class RestPasswordCommand : IRequest<AuthResult>
    {
        public RestPasswordViewModel PasswordViewModel { get; set; }
        public ClaimsPrincipal claims { get; set; }
    }
    public class RestPasswordCommandHandler : IRequestHandler<RestPasswordCommand, AuthResult>
    {
        private readonly IIdentityService identityService;

        public RestPasswordCommandHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<AuthResult> Handle(RestPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.RestPasswordAsync( request.PasswordViewModel,request.claims.GetUserId());
            return result;
        }
    }
}
