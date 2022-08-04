using Khandon.Core.CQS.Command;
using Khandon.Core.CQS.Query;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
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
    public class BookController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Routes.V1.User.Book.GetBookList)]
        public async Task<ActionResult<BookListResponse>> GetBookList()
        {
            var result = await mediator.Send(new GetBookListQuery()
            {
                User=User
            });
            return Ok(result);
        }

        [HttpPost(Routes.V1.User.Book.CreateBook)]
        public async Task<ActionResult<BookResult>> CreateBook([FromBody] BookCreate bookCreate)
        {
            var result = await mediator.Send(new BookCreateCommand()
            {
                User = User,
                BookCreate = bookCreate
            });
            return Ok(result);
        }

        [HttpPut(Routes.V1.User.Book.EditBook)]
        public async Task<ActionResult<BookResult>> EditBook([FromRoute] Guid Id, [FromBody] BookEdit bookEdit)
        {
            var result = await mediator.Send(new BookEditCommand()
            {
                User = User,
                BookId = Id,
                BookEdit = bookEdit
            });
            return Ok(result);
        }
        [HttpDelete(Routes.V1.User.Book.DeleteBook)]
        public async Task<ActionResult<Task>> DeleteBook([FromRoute] Guid Id)
        {
            var result = await mediator.Send(new BookDeleteCommand()
            {
                User = User,
                BookId = Id
            });
            return Ok(result);
        }

    }
}