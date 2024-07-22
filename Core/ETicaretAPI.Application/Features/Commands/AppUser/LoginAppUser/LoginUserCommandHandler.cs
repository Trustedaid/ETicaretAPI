﻿using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginAppUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

    public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager,
        SignInManager<Domain.Entities.Identity.AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request,
        CancellationToken cancellationToken)
    {
       var user =  await _userManager.FindByNameAsync(request.UsernameOrEmail);
       if (user == null)
           user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
       if(user == null)
           throw new UserNotFoundException("Username or password is invalid");

       var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
       if (result.Succeeded)
       {
           //TODO: Authorization need to be specified
       }
       return new();
    }
}