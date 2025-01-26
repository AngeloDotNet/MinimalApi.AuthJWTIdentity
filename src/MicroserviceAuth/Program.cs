using MicroserviceAuth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static MicroserviceAuth.Models.Requests;

namespace MicroserviceAuth
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseInMemoryDatabase("databaseInMemory");
			});

			builder.Services.AddSwaggerAuth();
			builder.Services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.AddAuthenticationJWT(builder.Configuration);

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapPost("/register", async (UserManager<IdentityUser> userManager, RegisterRequest request) =>
			{
				var user = new IdentityUser { UserName = request.Username, Email = request.Email };
				var result = await userManager.CreateAsync(user, request.Password);

				if (result.Succeeded)
				{
					return Results.Ok();
				}

				return Results.BadRequest(result.Errors);
			});

			app.MapPost("/login", async (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, LoginRequest request) =>
			{
				var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					var user = await userManager.FindByNameAsync(request.Username);
					var token = GenerateJwtToken(user!);

					return Results.Ok(new { Token = token });
				}

				return Results.Unauthorized();
			});

			string GenerateJwtToken(IdentityUser user)
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, user.UserName!)]),
					Expires = DateTime.UtcNow.AddDays(7),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
					Issuer = builder.Configuration["Jwt:Issuer"],
					Audience = builder.Configuration["Jwt:Audience"]
				};

				var token = tokenHandler.CreateToken(tokenDescriptor);

				return tokenHandler.WriteToken(token);
			}

			app.MapGet("/secure-endpoint", [Authorize] () => "This is a secure endpoint");

			app.Run();
		}
	}
}