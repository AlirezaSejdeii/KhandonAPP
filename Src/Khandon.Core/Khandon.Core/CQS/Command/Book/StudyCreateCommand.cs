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

namespace Khandon.Core.CQS.Command
{
    public class StudyCreateCommand:IRequest<StudyResult>
    {
        public ClaimsPrincipal User { get; set; }
        public StudyCreate StudyCreate { get; set; }
    }

    public class StudyCreateCommandHandler : IRequestHandler<StudyCreateCommand, StudyResult>
    {
        private readonly IStudyRepository _studyRepository;

        public StudyCreateCommandHandler(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository;
        }

        public async Task<StudyResult> Handle(StudyCreateCommand request, CancellationToken cancellationToken)
        {
            string UserId = request.User.GetUserId();

            StudyResult studyResult = await _studyRepository.CreateStudyAsync(request.StudyCreate, UserId);

            return studyResult;
        }
    }
}
