using System.Text.Json;
using ETicaretAPI.Application.Abstractions.JWT;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;

public class
    FacebookLoginUserCommandHandler : IRequestHandler<FacebookLoginUserCommandRequest, FacebookLoginUserCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly HttpClient _httpClient;

    public FacebookLoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager,
        ITokenHandler tokenHandler, HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _httpClient = httpClient;
    }

    public async Task<FacebookLoginUserCommandResponse> Handle(FacebookLoginUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var accessTokenResponse = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/v11.0/oauth/access_token?client_id=509559531586453&client_secret=756d241c84378cd0f7f2ef1f1c17be44&grant_type=client_credentials");
        // await _httpClient.GetStringAsync($"https://graph.facebook.com/v11.0/oauth/access_token?client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&grant_type=client_credentials");

        var facebookAccessTokenResponse =
            JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

        var userAccessTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponse.AccessToken}");

        var validation = JsonSerializer.Deserialize<FacebookUserAccessValidation>(userAccessTokenValidation);

        if (validation.Data.IsValid)
        {
            var userInfoResponse = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/v11.0/me?fields=id,email,first_name,last_name&access_token={request.AuthToken}");
            var userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);


            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            var result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = userInfo.Name,
                        Email = userInfo.Email,
                        UserName = userInfo.Email
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);


                Token token = _tokenHandler.CreateAccessToken(1440);
                return new()
                {
                    Token = token
                };
            }
        }

        throw new Exception("Invalid external authentication");
    }
}