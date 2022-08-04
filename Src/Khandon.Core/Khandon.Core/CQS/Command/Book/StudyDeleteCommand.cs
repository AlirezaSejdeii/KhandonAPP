using Khandon.Core.Interfaces.IRepository;
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
    public class StudyDeleteCommand : IRequest<bool>
    {
        public ClaimsPrincipal User { get; set; }
        public Guid StudyId { get; set; }
    }

    public class StudyDeleteCommandHandler : IRequestHandler<StudyDeleteCommand, bool>
    {
        private readonly IStudyRepository studyRepository;

        public StudyDeleteCommandHandler(IStudyRepository studyRepository)
        {
            this.studyRepository = studyRepository;
        }

        public async Task<bool> Handle(StudyDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await studyRepository.SafeDeleteStudyAsync(request.User.GetUserId(), request.StudyId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
