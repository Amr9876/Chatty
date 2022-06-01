
using Chatty.Data;

namespace Chatty.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Models;

public class AuthService
{

    private readonly AppDbContext db;

    private readonly ProtectedLocalStorage localStorage;

    private readonly IConfiguration config;

    public AuthService(ProtectedLocalStorage localStorage, IConfiguration config)
    {
        db = new();
        this.localStorage = localStorage;
        this.config = config;
    }

    public async Task AuthenticateAsync(LoginRequest request) 
    {

        var user = await db.Users.SingleOrDefaultAsync(x => x.EmailAddress == request.Email && x.Password == request.Password);

        if (user is not null)
        {

            var claims = new Claim[] 
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(config["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(claims: claims, signingCredentials: credentials, expires: DateTime.Now.AddDays(3));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            await localStorage.SetAsync("access_token", token);

        }

    }

    public async Task CreateUserAsync(RegisterRequest request)
    {

        if(request is not null)
        {

            var user = new User 
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.Email,
                Password = request.Password
            };

            user.FullName = string.Format("{0} {1}", user.FirstName, user.LastName);

            db.Users.Add(user);
            await db.SaveChangesAsync();

        }

    }

}
