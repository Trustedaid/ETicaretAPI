using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginAppUser;

public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
{
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
}