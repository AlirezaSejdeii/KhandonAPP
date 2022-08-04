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
    public class GetChapterByBookIdQuery : IRequest<List<ChapterResult>>
    {
        public ClaimsPrincipal User { get; set; }
        public GetChapterByBookId GetChapter { get; set; }
    }

    public class GetChapterByBookIdQueryHandler : IRequestHandler<GetChapterByBookIdQuery, List<ChapterResult>>
    {
        private readonly IChapterRepository chapterRepository;

        public GetChapterByBookIdQueryHandler(IChapterRepository chapterRepository)
        {
            this.chapterRepository = chapterRepository;
        }

        public async Task<List<ChapterResult>> Handle(GetChapterByBookIdQuery request, CancellationToken cancellationToken)
        {
            var result = await chapterRepository.GetChaptersByBookIdAsync(request.GetChapter.BookId, request.User.GetUserId(), request.GetChapter.limit, request.GetChapter.page);
            return result;
        }
    }
}
