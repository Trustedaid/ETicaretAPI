﻿using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
    Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenExpiration, int accessTokenPlusTime);
    Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
}