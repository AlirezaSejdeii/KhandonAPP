using Khandon.Core.CQS.Command;
using Khandon.Core.CQS.Query;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khandon.Persentation.API.Controllers.User.V1
{
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "User")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChapterController : ControllerBase
    {
        private readonly IMediator mediator;

        public ChapterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Routes.V1.User.Chapter.GetChapterById)]
        public async Task<ActionResult<ChapterResult>> GetChapterById(Guid Id)
        {
            var result= await mediator.Send(new GetChapterByIdQuery()
            {
                User = User,
                ChapterId=Id
            });
            return result;
        }

        [HttpGet(Routes.V1.User.Chapter.GetChapterBookId)]
        public async Task<ActionResult<List<ChapterResult>>> GetChapterById([FromQuery] GetChapterByBookId chapterByBookId)
        {
            var result = await mediator.Send(new GetChapterByBookIdQuery()
            {
                User = User,
                GetChapter = chapterByBookId
            }) ;
            return result;
        }

        [HttpPost(Routes.V1.User.Chapter.CreateChapter)]
        public async Task<ActionResult<ChapterResult>> CreateChapter([FromBody] ChapterCreate chapterCreate)
        {            
            var result = await mediator.Send(new ChapterCreateCommand()
            {
                User = User,
                ChapterCreate = chapterCreate
            });
            return result;
        }

        [HttpPut(Routes.V1.User.Chapter.EditChapter)]
        public async Task<ActionResult<ChapterResult>> EditChapter([FromBody] ChapterEdit chapterEdit)
        {
            var result = await mediator.Send(new ChapterEditCommand()
            {
                ChapterEdit = chapterEdit,
                User = User
            });
            return result;
        }

        [HttpDelete(Routes.V1.User.Chapter.DeleteChapter)]
        public async Task<ActionResult<bool>> DeleteChapter([FromRoute] Guid Id)
        {
            bool result = await mediator.Send(new ChapterDeleteCommand()
            {
                User = User,
                ChapterId = Id
            });
            return result;
        }
    }
}
