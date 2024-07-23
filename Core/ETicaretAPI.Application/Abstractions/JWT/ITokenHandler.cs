using ETicaretAPI.Application.DTOs;

namespace ETicaretAPI.Application.Abstractions.JWT;

public interface ITokenHandler
{
    Token CreateAccessToken(int minute);
    
    
}