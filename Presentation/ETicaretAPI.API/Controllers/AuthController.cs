using ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginAppUser;
using ETicaretAPI.Application.Features.Commands.AppUser.PasswordReset;
using ETicaretAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.VerifyResetToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            var response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin(
            [FromBody] RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)

        {
            var response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("Google-Login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginUserCommandRequest googleLoginUserCommandRequest)
        {
            var response = await _mediator.Send(googleLoginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("Facebook-Login")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginUserCommandRequest facebookLoginUserCommandRequest)
        {
            var response = await _mediator.Send(facebookLoginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(
            [FromBody] PasswordResetCommandRequest passwordResetCommandRequest)
        {
            var response = await _mediator.Send(passwordResetCommandRequest);
            return Ok(response);
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken(
            [FromBody] VerifyResetTokenCommandRequest verifyResetTokenCommandRequest)
        {
            var response = await _mediator.Send(verifyResetTokenCommandRequest);
            return Ok(response);
        }
    }
}