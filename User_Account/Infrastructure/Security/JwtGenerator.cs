using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Infrastructure.Security;

public class JwtGenerator : IJwtGenerator
{
    private readonly DataContext _context;
    private readonly SymmetricSecurityKey _key;

    public JwtGenerator(IConfiguration config, DataContext context)
    {
        _context = context;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:TokenSecret"]));
    }
    
    public async Task<string> CreateToken(AppUser user, CancellationToken cancellationToken = default)
    {
        var userExist = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == user.Email && !user.Archived, cancellationToken);

        if (userExist is null)
            throw new Exception("User not found or is Archived");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}