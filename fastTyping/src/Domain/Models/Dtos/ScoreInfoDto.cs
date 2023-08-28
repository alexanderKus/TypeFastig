using System;
namespace Domain.Models.Dtos;

public record ScoreInfoDto(string Username, float Accuracy, int Speed);
