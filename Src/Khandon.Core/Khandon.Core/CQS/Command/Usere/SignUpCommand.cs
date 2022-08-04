using Khandon.Core.Interfaces.User.Identity;
using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Command.Usere
{
    public class SignUpCommand:IRequest<AuthResult>
    {
        public UserSignUp userSignUp { get; set; }
    }
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthResult>
    {
        private readonly IIdentityService identityService;

        public SignUpCommandHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<AuthResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.UserSignUpAsync(request.userSignUp);
            return result;
        }
    }
}
