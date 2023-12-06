using System;
using Domain.Models.Enums;
namespace Domain.Models.Dtos;

public record ScoreInfoDto(string Username, float Accuracy, int Speed, Language Language);
