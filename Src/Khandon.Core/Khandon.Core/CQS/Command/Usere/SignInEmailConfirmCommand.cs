using Khandon.Core.Interfaces.User.Identity;
using Khandon.Shared.Dto.Base.User;
using Khandon.Shared.Dto.Response.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Command.Usere
{
    public class SignInEmailConfirmCommand:IRequest<AuthResult>
    {
        public ConfirmationModelDto confirmationModel { get; set; }
    }
    public class SignInEmailConfirmCommandHandler : IRequestHandler<SignInEmailConfirmCommand, AuthResult>
    {
        private readonly IIdentityService identityService;

        public SignInEmailConfirmCommandHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<AuthResult> Handle(SignInEmailConfirmCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.ConfirmEmailAsync(request.confirmationModel.Token, request.confirmationModel.Email);
            return result;
                
        }
    }
}
