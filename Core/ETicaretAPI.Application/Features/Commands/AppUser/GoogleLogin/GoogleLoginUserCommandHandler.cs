using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;

public class
    GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommandRequest, GoogleLoginUserCommandResponse>
{
    private readonly IAuthService _authService;
    

    public GoogleLoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<GoogleLoginUserCommandResponse> Handle(GoogleLoginUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _authService.GoogleLoginAsync(request.IdToken, 1440);
        return new GoogleLoginUserCommandResponse()
        {
            Token = token
        };
    }
}