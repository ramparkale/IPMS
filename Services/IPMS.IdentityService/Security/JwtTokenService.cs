using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IPMS.IdentityService.Entities;
using Microsoft.IdentityModel.Tokens;

namespace IPMS.IdentityService.Security;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(
        User user,
        List<string> roles,
        List<string> permissions)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = jwtSettings["Key"]
            ?? throw new InvalidOperationException("JWT key missing.");

        var issuer = jwtSettings["Issuer"];

        var audience = jwtSettings["Audience"];

        var expiryMinutes =
            int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.UserId.ToString()),

            new(
                JwtRegisteredClaimNames.UniqueName,
                user.UserName),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email),

            new(
                ClaimTypes.NameIdentifier,
                user.UserId.ToString()),

            new(
                ClaimTypes.Name,
                user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(ClaimTypes.Role, role));
        }

        foreach (var permission in permissions)
        {
            claims.Add(
                new Claim("permission", permission));
        }

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return
        (
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt
        );
    }
}