using ETicaretAPI.Application.DTOs;

namespace ETicaretAPI.Application.Abstractions.Services.Authentication;

public interface IExternalAuthentication
{
    Task<Token> FacebookLoginAsync(string authToken, int accessTokenExpiration);
    Task<Token> GoogleLoginAsync(string idToken, int accessTokenExpiration);
}