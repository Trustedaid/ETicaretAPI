﻿using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.VerifyResetToken;

public class
    VerifyResetTokenCommandHandler : IRequestHandler<VerifyResetTokenCommandRequest, VerifyResetTokenCommandResponse>
{
    private readonly IAuthService _authService;

    public VerifyResetTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<VerifyResetTokenCommandResponse> Handle(VerifyResetTokenCommandRequest request,
        CancellationToken cancellationToken)
    {
      var state = await _authService.VerifyResetTokenAsync(request.ResetToken, request.UserId);
        return new VerifyResetTokenCommandResponse()
        {
            State = true
        };
    }
}