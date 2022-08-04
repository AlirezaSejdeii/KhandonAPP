using Khandon.Core.CQS.Command;
using Khandon.Core.CQS.Query;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Resposne;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khandon.Persentation.API.Controllers.Admin.V1
{
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookGroupController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookGroupController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet(Routes.V1.Admin.BookGroup.GetAllBookGroup)]
        public async Task<ActionResult<List<BookGroupResult>>> GetAllBookGroup()
        {
            var result = await mediator.Send(new GetBookGroupsQuery());
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
        [HttpPost(Routes.V1.Admin.BookGroup.CreateBookGroup)]
        public async Task<ActionResult<BookGroupResult>> CreateBookGroup([FromBody] BookGroupCreate bookGroupCreate)
        {
            var result = await mediator.Send(new BookGroupCreateCommand()
            {
                BookGroupCreate = bookGroupCreate
            });
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut(Routes.V1.Admin.BookGroup.EditBookGroup)]
        public async Task<ActionResult<BookGroupResult>> EditBookGroup([FromBody] BookGroupEdit bookGroupEdit)
        {
            var result = await mediator.Send(new BookGroupEditCommand
                ()
            {
                BookGroupEdit = bookGroupEdit                
            });
            return Ok(result);
        }
    }
}
