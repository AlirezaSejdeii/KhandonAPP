using Khandon.Core.CQS.Command;
using Khandon.Core.CQS.Query;
using Khandon.Shared.Dto.Enums;
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
    public class StudyController : ControllerBase
    {
        private readonly IMediator mediator;

        public StudyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Routes.V1.User.Study.GetStudyById)]
        public async Task<ActionResult<StudyResult>> GetStudyById([FromRoute] Guid Id)
        {
            var result = await mediator.Send(new GetStudyByIdQuery()
            {
                User=User,
                StudyId=Id
            });
            return result;
        }

        [HttpGet(Routes.V1.User.Study.GetStudy)]
        public async Task<ActionResult<List<StudyResult>>> GetStudy([FromQuery] StudyGet studyGet)
        {
            var result = await mediator.Send(new GetStudiesQuery()
            {
                StudyGet = studyGet,
                User = User
            });
            return result;
        }

        [HttpGet(Routes.V1.User.Study.GetSocietyStatus)]
        public async Task<SocietyStatusDto> GetSocietyStatus()
        {
            return await mediator.Send(new GetSocietyStatusAsyncQuery()
            {
                Claims=User
            });
        }
        [HttpPost(Routes.V1.User.Study.CreateStudy)]
        public async Task<ActionResult<StudyResult>> CreateStudy([FromBody] StudyCreate studyCreate)
        {
            var result = await mediator.Send(new StudyCreateCommand()
            {
                StudyCreate = studyCreate,
                User = User
            });
            return result;
        }

        [HttpPut(Routes.V1.User.Study.EditStudy)]
        public async Task<ActionResult<StudyResult>> EditStudy([FromBody] StudyEdit studyEdit)
        {
            var result = await mediator.Send(new StudyEditCommand()
            {
                StudyEdit = studyEdit,
                User = User
            });
            return result;
        }

        [HttpDelete(Routes.V1.User.Study.DeleteStudy)]
        public async Task<ActionResult<bool>> DeleteStudy([FromRoute] Guid Id)
        {
            var result = await mediator.Send(new StudyDeleteCommand()
            {
                StudyId = Id,
                User = User
            });
            return result;
        }
        [HttpGet(Routes.V1.User.Study.GetPeriodTimeSocityActivity)]
        public async Task<ActionResult<List<StatusActiveReadsDto>>> GetPeriodTimeSocityActivity([FromQuery] ReadingActivityDateEnum time, [FromQuery] int limit)
        {
            var result = await mediator.Send(new GetPeriodTimeSocityActivityQuery()
            {
                limit = limit,
                time = time
            });
            return result;
        }
    }
}
