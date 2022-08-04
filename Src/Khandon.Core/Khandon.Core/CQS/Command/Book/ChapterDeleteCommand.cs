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
    public class ChapterDeleteCommand : IRequest<bool>
    {
        public ClaimsPrincipal User { get; set; }
        public Guid ChapterId { get; set; }
    }
    public class ChapterDeleteCommandHandler : IRequestHandler<ChapterDeleteCommand, bool>
    {
        private readonly IChapterRepository chapterRepository;

        public ChapterDeleteCommandHandler(IChapterRepository chapterRepository)
        {
            this.chapterRepository = chapterRepository;
        }

        public async Task<bool> Handle(ChapterDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                chapterRepository.SafeDeleteChapter(request.ChapterId, request.User.GetUserId());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
