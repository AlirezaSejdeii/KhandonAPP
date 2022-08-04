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
    public class ChapterCreateCommand:IRequest<ChapterResult>
    {
        public ClaimsPrincipal User { get; set; }
        public ChapterCreate ChapterCreate { get; set; }
    }

    public class ChapterCreateCommandHandler : IRequestHandler<ChapterCreateCommand
        , ChapterResult>
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterCreateCommandHandler(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<ChapterResult> Handle(ChapterCreateCommand request, CancellationToken cancellationToken)
        {
            string UserId = request.User.GetUserId();

            ChapterResult chapterResult= await  _chapterRepository.CreateChapterAsync(request.ChapterCreate, UserId);

            return chapterResult;
        }
    }
}
