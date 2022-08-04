using Khandon.Core.Interfaces.User.Identity;
using Khandon.Shared.Dto.Response.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Query.User
{
    public class RecovryPasswordQuery:IRequest<AuthResult>
    {
        public string EmailOrUsername { get; set; }
    }
    public class RecovryPasswordQueryHandler : IRequestHandler<RecovryPasswordQuery, AuthResult>
    {
        private readonly IIdentityService identityService;

        public RecovryPasswordQueryHandler(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        async Task<AuthResult> IRequestHandler<RecovryPasswordQuery, AuthResult>.Handle(RecovryPasswordQuery request, CancellationToken cancellationToken)
        {
            var result = await identityService.RecoveryPasswordAsync(request.EmailOrUsername);
            return result;
        }
    }
}
