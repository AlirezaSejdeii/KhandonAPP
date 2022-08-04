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
    public class StudyEditCommand : IRequest<StudyResult>
    {
        public ClaimsPrincipal User { get; set; }
        public StudyEdit StudyEdit { get; set; }
    }

    public class StudyEditCommandHandler : IRequestHandler<StudyEditCommand, StudyResult>
    {
        private readonly IStudyRepository _studyRepository;

        public StudyEditCommandHandler(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository;
        }

        public async Task<StudyResult> Handle(StudyEditCommand request, CancellationToken cancellationToken)
        {
            string UserId = request.User.GetUserId();

            StudyResult studyResult = await _studyRepository.EditStudyAsync(request.StudyEdit, UserId);

            return studyResult;
        }
    }
}
