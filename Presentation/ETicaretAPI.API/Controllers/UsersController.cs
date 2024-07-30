using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateAppUser;
using ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginAppUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
    {
        var response = await _mediator.Send(createUserCommandRequest);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
    {
        var response = await _mediator.Send(loginUserCommandRequest);
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
}