using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Server.Dto;

namespace Server;

public interface IUserService
{
    IEnumerable<User> Users { get; }
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, CancellationToken cancellationToken);
}

public class UserService : IUserService
{
    private static IList<User> users = new List<User>();

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, CancellationToken cancellationToken)
    {
        var user = users.FirstOrDefault(x => x.Name.Equals(model.Name));
        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            };
            users.Add(user);
        }
        var token = GenerateJwtToken(user);

        return await Task.FromResult(new AuthenticateResponse(user, token));
    }

    public IEnumerable<User> Users => users;

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("thisismysupersecretpasswordthisismysupersecretpassword");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "role1"),
                new Claim(ClaimTypes.Role, "role2")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}