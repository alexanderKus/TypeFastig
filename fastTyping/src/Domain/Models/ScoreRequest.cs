namespace Domain.Models;

public record ScoreRequest(int UserId, float Precision, TimeSpan Time);
