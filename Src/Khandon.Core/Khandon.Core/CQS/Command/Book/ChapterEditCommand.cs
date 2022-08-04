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
    public class ChapterEditCommand : IRequest<ChapterResult>
    {
        public ClaimsPrincipal User { get; set; }
        public ChapterEdit ChapterEdit { get; set; }
    }

    public class ChapterEditCommandHandler : IRequestHandler<ChapterEditCommand
        , ChapterResult>
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterEditCommandHandler(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<ChapterResult> Handle(ChapterEditCommand request, CancellationToken cancellationToken)
        {
            string UserId = request.User.GetUserId();

            ChapterResult chapterResult = await _chapterRepository.EditChapterAsync(request.ChapterEdit, UserId);

            return chapterResult;
        }
    }
}
