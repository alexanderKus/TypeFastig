using Domain.Models.Enums;

namespace Domain.Models;

public record ScoreRequest(int UserId, float Accuracy, int Speed, Language Language);
