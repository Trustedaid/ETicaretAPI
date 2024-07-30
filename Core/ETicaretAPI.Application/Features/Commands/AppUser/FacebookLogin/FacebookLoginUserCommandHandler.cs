using System.Text.Json;
using ETicaretAPI.Application.Abstractions.JWT;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;

public class
    FacebookLoginUserCommandHandler : IRequestHandler<FacebookLoginUserCommandRequest, FacebookLoginUserCommandResponse>
{
    private readonly IAuthService _authService;

    public FacebookLoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FacebookLoginUserCommandResponse> Handle(FacebookLoginUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _authService.FacebookLoginAsync(request.AuthToken, 1440);

        return new FacebookLoginUserCommandResponse
        {
            Token = token
        };
    }
}