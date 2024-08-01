using ETicaretAPI.Application.DTOs;

namespace ETicaretAPI.Application.Abstractions.Services.Authentication;

public interface IInternalAuthentication
{
    Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenExpiration);
    Task<Token> RefreshTokenLoginAsync(string refreshToken);
}