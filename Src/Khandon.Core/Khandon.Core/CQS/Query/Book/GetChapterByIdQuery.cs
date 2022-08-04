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
    public class GetChapterByIdQuery : IRequest<ChapterResult>
    {
        public ClaimsPrincipal User { get; set; }
        public Guid ChapterId { get; set; }
    }

    public class GetChapterByIdQueryHandler : IRequestHandler<GetChapterByIdQuery, ChapterResult>
    {
        private readonly IChapterRepository chapterRepository;

        public GetChapterByIdQueryHandler(IChapterRepository chapterRepository)
        {
            this.chapterRepository = chapterRepository;
        }

        public async Task<ChapterResult> Handle(GetChapterByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await chapterRepository.GetChapterByIdAsync(request.ChapterId, request.User.GetUserId());
            return result;
        }
    }
}
