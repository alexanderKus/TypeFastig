namespace Domain.Models.Auth;

public record RegisterRequest(string Username, string Password, string Email);
