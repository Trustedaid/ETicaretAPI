using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateAppUser;
using ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginAppUser;
using ETicaretAPI.Application.Features.Commands.AppUser.UpdatePassword;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMailService _mailService;

    public UsersController(IMediator mediator, IMailService mailService)
    {
        _mediator = mediator;
        _mailService = mailService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
    {
        var response = await _mediator.Send(createUserCommandRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> ExampleMailTest()
    {
        await _mailService.SendMailAsync("oguzern@gmail.com",  "Portal Mail Service Activated", "<strong> Test mail message works for html codes as well <3 </strong>");
        return Ok();
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
    {
        var response = await _mediator.Send(updatePasswordCommandRequest);
        return Ok(response);
    }
   

   
}