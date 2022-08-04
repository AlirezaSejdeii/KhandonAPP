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
    public class RecoveryPasswordConfirmCommand:IRequest<AuthResult>
    {
        public RecoveryPasswordConfirmViewModel recoveryPassword { get; set; }
    }

    public class RecoveryPasswordConfirmCommandHandler : IRequestHandler<RecoveryPasswordConfirmCommand, AuthResult>
    {
        private readonly IIdentityService identityService;

        public RecoveryPasswordConfirmCommandHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<AuthResult> Handle(RecoveryPasswordConfirmCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.RecoveryPasswordConfirmAsync(request.recoveryPassword);
            return result;
        }
    }
}
