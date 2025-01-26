namespace MicroserviceAuth.Models;

public class Requests
{
	public record RegisterRequest(string Username, string Email, string Password);
	public record LoginRequest(string Username, string Password);
}