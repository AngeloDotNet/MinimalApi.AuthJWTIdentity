using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;

namespace MicroserviceAuth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerAuth();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("IdentityMemoryDatabase"));

        builder.Services.AddAuthenticationJWT();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapPost("/login", async (LoginRequest request, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) =>
        {
            if (request.Username == "admin" && request.Password == "admin")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(JWTConfiguration.SecurityKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([
                        new(ClaimTypes.NameIdentifier, "aa787598-2faa-4d8d-b102-2b5f5de364c8"),
                        new(ClaimTypes.Name, "admin")
                    ]),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                    Issuer = JWTConfiguration.Issuer,
                    Audience = JWTConfiguration.Audience
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Results.Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            return Results.Unauthorized();
        }).AllowAnonymous();
    }
}