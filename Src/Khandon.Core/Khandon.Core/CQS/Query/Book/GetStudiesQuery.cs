using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Request;
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
    public class GetStudiesQuery : IRequest<List<StudyResult>>
    {
        public ClaimsPrincipal User { get; set; }
        public StudyGet StudyGet { get; set; }
    }

    public class GetStudiesQueryHandler : IRequestHandler<GetStudiesQuery, List<StudyResult>>
    {
        private readonly IStudyRepository _studyRepository;

        public GetStudiesQueryHandler(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository;
        }

        public async Task<List<StudyResult>> Handle(GetStudiesQuery request, CancellationToken cancellationToken)
        {
            string UserId = request.User.GetUserId();
            List<StudyResult> studyResults= await _studyRepository.GetStudyListAsync(UserId, request.StudyGet);

            return studyResults;
        }
    }
}
