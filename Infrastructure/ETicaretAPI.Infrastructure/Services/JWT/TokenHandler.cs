using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ETicaretAPI.Application.Abstractions.JWT;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ETicaretAPI.Infrastructure.Services.JWT;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Token CreateAccessToken(int second, AppUser user)
    {
        Token token = new();
        //SecurityKey'in simetriğini oluşturuyoruz.
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

        //Şifrelenmiş kimliği oluşturuyoruz.
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        // Oluşturulacak Token ayarlarını veriyoruz.
        token.Expiration = DateTime.UtcNow.AddSeconds(second);

        JwtSecurityToken securityToken = new(
            audience: _configuration["Token:Audience"],
            issuer: _configuration["Token:Issuer"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials,
            claims: new List<Claim>{new (ClaimTypes.Name, user.UserName)}
        );

        //Token oluşturucu sınıfını kullanarak bir örnek alıyoruz.
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);
        
        token.RefreshToken = CreateRefreshToken();

        return token;
    }

    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(number);


        return Convert.ToBase64String(number);
    }
}