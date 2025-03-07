﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Application.Repositories.Endpoint;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEndpointReadRepository _endpointReadRepository;

    public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
    {
        _userManager = userManager;
        _endpointReadRepository = endpointReadRepository;
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

    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenExpiration,
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

    public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            resetToken = resetToken.UrlDecode();
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user);
            else
                throw new PasswordChangeFailedException();
        }
    }

    public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
    {
        var users = await _userManager.Users.Skip(page * size).Take(size).ToListAsync();

        return users.Select(user => new ListUser()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TwoFactorEnabled = user.TwoFactorEnabled,
            }
        ).ToList();
    }

    public int TotalUsersCount => _userManager.Users.Count();

    public async Task AssignRoleToUserAsync(string userId, string[] role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRolesAsync(user, userRoles);
        }
    }

    public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
    {
        var user = await _userManager.FindByIdAsync(userIdOrName);
        if (user == null)
        {
            await _userManager.FindByNameAsync(userIdOrName);
        }

        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }

        return new string[] { };
        // return Array.Empty<string>() suggested;
    }

    public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
    {
        var userRoles = await GetRolesToUserAsync(name);
        if (!userRoles.Any())
            return false;
        var endpoint = await _endpointReadRepository.Table
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code);
        if (endpoint == null)
            return false;

        var hasRole = false;
        var endpointRoles = endpoint.Roles.Select(r => r.Name);

        // foreach (var userRole in userRoles)
        // {
        //     if (!hasRole)
        //     {
        //         foreach (var endpointRole in endpointRoles)
        //         {
        //             if (userRole == endpointRole)
        //             {
        //                 hasRole = true;
        //                 break;
        //             }
        //         }
        //     }
        //     else
        //         break;
        // }
        //
        // return hasRole;

        foreach (var userRole in userRoles)
        {
            foreach (var endpointRole in endpointRoles)
                if (userRole == endpointRole)
                    return true;
        }

        return false;

    }
}