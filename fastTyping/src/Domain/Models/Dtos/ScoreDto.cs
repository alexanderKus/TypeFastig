using Domain.Models.Enums;

namespace Domain.Models.Dtos;

public record ScoreDto(int Id, float Accuracy, int Speed, Language Language);
