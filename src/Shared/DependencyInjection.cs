using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Shared;

public static class DependencyInjection
{
	public static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Insert the Bearer Token",
				Name = HeaderNames.Authorization,
				Type = SecuritySchemeType.ApiKey
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference= new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme
						}
					},
					Array.Empty<string>()
				}
			});
		});

		return services;
	}

	public static IServiceCollection AddAuthenticationJWT(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = configuration["Jwt:Issuer"],
				ValidAudience = configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
			};
		});

		services.AddAuthorization();

		return services;
	}
}