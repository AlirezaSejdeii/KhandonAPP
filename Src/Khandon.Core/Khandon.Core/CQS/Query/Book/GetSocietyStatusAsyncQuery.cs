using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Query
{
    public class GetSocietyStatusAsyncQuery:IRequest<SocietyStatusDto>
    {
        public ClaimsPrincipal Claims { get; set; }
    }
    public class GetSocietyStatusAsyncQueryHandler : IRequestHandler<GetSocietyStatusAsyncQuery, SocietyStatusDto>
    {
        private readonly IStudyRepository studyRepository;

        public GetSocietyStatusAsyncQueryHandler(IStudyRepository studyRepository)
        {
            this.studyRepository = studyRepository;
        }

        public async Task<SocietyStatusDto> Handle(GetSocietyStatusAsyncQuery request, CancellationToken cancellationToken)
        {
            return await studyRepository.GetSocietyStatusAsync(request.Claims.GetUserId());
        }
    }
}
