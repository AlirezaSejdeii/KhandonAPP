using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Result;
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
    public class GetStudyByIdQuery : IRequest<StudyResult>
    {
        public Guid StudyId { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
    public class GetStudyByIdQueryHandler : IRequestHandler<GetStudyByIdQuery, StudyResult>
    {
        private readonly IStudyRepository _studyRepository;

        public async Task<StudyResult> Handle(GetStudyByIdQuery request, CancellationToken cancellationToken)
        {
            string UserId = request.User.GetUserId();
            StudyResult studyResult = await _studyRepository.GetStudyResultById(request.StudyId,UserId);
            return studyResult;
        }
    }
}
