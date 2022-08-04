using Khandon.Core.CQS.Command.Usere;
using Khandon.Core.CQS.Query.User;
using Khandon.Persentation.API.Helper.Authorization;
using Khandon.Shared.Dto.Base.User;
using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khandon.Persentation.API.Controllers.User.V1
{
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "User")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Routes.V1.User.Users.SignUpUser)]
        public async Task<ActionResult<AuthResult>> SignUpUser([FromBody] UserSignUp userSignUp)
        {
            var result = await mediator.Send(new SignUpCommand()
            {
                userSignUp = userSignUp
            });
            return Ok(result);
        }

        [HttpPost(Routes.V1.User.Users.LoginInUser)]
        public async Task<ActionResult<AuthResult>> LoginInUser([FromBody] UserLogin userLogin)
        {
            var result = await mediator.Send(new UserLoginQuery()
            {
                userLogin = userLogin
            });
            return Ok(result);
        }

        [HttpGet(Routes.V1.User.Users.ConfirmAccount)]
        public async Task<ActionResult<AuthResult>> ConfirmAccountEmail([FromQuery] ConfirmationModelDto confirmationModel)
        {
            var result = await mediator.Send(new SignInEmailConfirmCommand() { confirmationModel = confirmationModel });
            return Ok(result);
        }

        [HttpPost(Routes.V1.User.Users.RecoveryPassword)]
        public async Task<ActionResult<AuthResult>> RecoveryPassword([FromBody] RecoveryPasswordModel  recoveryPassword)
        {
            var result = await mediator.Send(new RecovryPasswordQuery()
            {
                EmailOrUsername = recoveryPassword.UsernameOrEmail
            });
            return result;
        }

        [HttpPost(Routes.V1.User.Users.RecoveryPasswordConfirm)]
        public async Task<ActionResult<AuthResult>> RecoveryPasswordConfirm([FromBody] RecoveryPasswordConfirmViewModel recovery)
        {
            var result = await mediator.Send(new RecoveryPasswordConfirmCommand()
            {
                recoveryPassword = recovery
            });
            return result;
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(Routes.V1.User.Users.RestPasssword)]
        public async Task<ActionResult<AuthResult>> RestPassword([FromBody] RestPasswordViewModel passwordViewModel)
        {
            var result = await mediator.Send(new RestPasswordCommand()
            {
                PasswordViewModel = passwordViewModel,
                claims=User
            });
            return result;
        }



    }
}
