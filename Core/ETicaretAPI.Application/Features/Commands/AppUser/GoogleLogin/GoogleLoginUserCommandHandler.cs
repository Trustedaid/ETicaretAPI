using ETicaretAPI.Application.Abstractions.JWT;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;

public class
    GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommandRequest, GoogleLoginUserCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;

    public GoogleLoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager,
        ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<GoogleLoginUserCommandResponse> Handle(GoogleLoginUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
             Audience = new List<string>{"YOUR_CLIENT_ID"}
            //Audience = new List<string> { "622372834950-f44qev133neei81frlgf69o22cu8a7kg.apps.googleusercontent.com" }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        var result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    UserName = payload.Email
                };
                var identityResult = await _userManager.CreateAsync(user);
              result = identityResult.Succeeded;
            }
        }

        if (result)
            await _userManager.AddLoginAsync(user, info);
        else
            Console.WriteLine("Invalid external login request");

        Token token = _tokenHandler.CreateAccessToken(1440);
        return new()
        {
            Token = token
        };
    }
}
