using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using smarttournamentengine.STE.DTOs;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Services;

public class AuthService(DatabaseContext context)
{
    private readonly DatabaseContext databaseContext = context;
    public async Task<User>? ValidateUser(UserDTO userDTO)
    {
        User? user = await databaseContext.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);

        if (user != null)
            if (user.Password == userDTO.Password) return new User
            {
                ID = user.ID,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                Role = user.Role
            };
        return null!;
    }
    public string GenerateJwttoken(User user)
    {
        var claims = new[] { new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.NameIdentifier, user.ID), new Claim(ClaimTypes.Role, user.Role) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Just a random strings put togther to get asymmetrical key"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(2), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}