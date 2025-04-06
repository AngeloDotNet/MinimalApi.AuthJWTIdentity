using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Microservice01;

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

        app.MapGet("/secure", [Authorize] () => "This is a secure endpoint");

        app.Run();
    }
}
