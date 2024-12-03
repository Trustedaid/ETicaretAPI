using System.Text;
using System.Text.Json;
using ETicaretAPI.Application.Abstractions.JWT;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager,
        IUserService userService, IMailService mailService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _userService = userService;
        _mailService = mailService;
        _httpClient = httpClientFactory.CreateClient();
    }

    private async Task<Token> CreateUserExternalAsync(AppUser user, string email, string firstName, string lastName,
        UserLoginInfo info,
        int accessTokenExpiration)
    {
        var result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email
                };
                var identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
        }

        if (result)
        {
            await _userManager.AddLoginAsync(user, info);
            var token = _tokenHandler.CreateAccessToken(accessTokenExpiration, user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 5);
            return token;
        }

        throw new Exception("Invalid external authentication");
    }

    public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenExpiration)
    {
        var accessTokenResponse = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:ClientId"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:ClientSecret"]}&grant_type=client_credentials");
        // await _httpClient.GetStringAsync($"https://graph.facebook.com/v11.0/oauth/access_token?client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&grant_type=client_credentials");

        var facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

        var userAccessTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

        var validation = JsonSerializer.Deserialize<FacebookUserAccessValidation>(userAccessTokenValidation);

        if (validation?.Data.IsValid != null)
        {
            var userInfoResponse = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/v11.0/me?fields=id,email,first_name,last_name&access_token={authToken}");
            var userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);


            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, userInfo.Name, info,
                accessTokenExpiration);
        }

        throw new Exception("Invalid external authentication");
    }


    public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenExpiration)
    {
        var user = await _userManager.FindByNameAsync(usernameOrEmail);

        if (user == null)
            user = await _userManager.FindByEmailAsync(usernameOrEmail);

        if (user == null)
            throw new UserNotFoundException();

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded)
        {
            var token = _tokenHandler.CreateAccessToken(accessTokenExpiration, user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
            return token;
        }


        throw new AuthenticationFailException();
    }

    public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        if (user != null && user.RefreshTokenEndDate > DateTime.UtcNow)
        {
            var token = _tokenHandler.CreateAccessToken(15, user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
            return token;
        }

        throw new UserNotFoundException("User not found");
    }

    public async Task PasswordResetAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // var tokenBytes = Encoding.UTF8.GetBytes(resetToken);
            // resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
            resetToken = resetToken.UrlEncode();

            await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
        }
    }

    public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // var tokenByte = WebEncoders.Base64UrlDecode(resetToken);
            // resetToken = Encoding.UTF8.GetString(tokenByte);
            resetToken = resetToken.UrlDecode();

            return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword", resetToken);
        }

        return false;
    }


    public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenExpiration)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string?> { _configuration["ExternalLoginSettings:Google:AppId"] }
            // Audience = new List<string> { "622372834950-f44qev133neei81frlgf69o22cu8a7kg.apps.googleusercontent.com" }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        return await CreateUserExternalAsync(user, payload.Email, payload.GivenName, payload.FamilyName, info,
            accessTokenExpiration);
    }
}