using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateAppUser;

public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}