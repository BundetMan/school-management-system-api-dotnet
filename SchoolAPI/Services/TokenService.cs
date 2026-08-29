using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SchoolAPI.Models.People;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;

    public TokenService(IConfiguration config, UserManager<User> userManager)
    {
        _config = config;
        _userManager = userManager;
    }
    public async Task<string> CreateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,    user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti,    Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier,      user.Id.ToString()),
            new(ClaimTypes.Name,                user.UserName??string.Empty)
        };
        if (string.IsNullOrEmpty(user.Email) == false)
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }
        if (string.IsNullOrEmpty(user.PhoneNumber) == false)
            claims.Add(new(ClaimTypes.MobilePhone, user.PhoneNumber));


        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        }

        var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

      var expiresInHours = _config.GetValue<int>("Jwt:ExpirationInHours", 8);

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:     _config["Jwt:Issuer"],
            audience:   _config["Jwt:Audience"],
            claims:     claims,
            expires:    DateTime.UtcNow.AddHours(expiresInHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}

