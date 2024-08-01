using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Persistence.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        var result = await _userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email
        }, model.Password);

        CreateUserResponse response = new() { Succeeded = result.Succeeded };

        if (result.Succeeded)
            response.Message = "User created successfully";

        else
        {
            foreach (var error in result.Errors)
            {
                response.Message += $"{error.Code}-{error.Description}";
            }
        }

        return response;
    }

    public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenExpiration,
        int accessTokenPlusTime)
    {
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenExpiration.AddSeconds(accessTokenPlusTime);
            await _userManager.UpdateAsync(user);
        }
        else
        {
            throw new UserNotFoundException("User not found");
        }
    }
}