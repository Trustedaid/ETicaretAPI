using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginUserCommandRequest : IRequest<FacebookLoginUserCommandResponse>
{
   public string AuthToken { get; set; }
}