using System;
namespace Domain.Models.Dtos;

public record Top100ScoresDto(
    List<ScoreInfoDto> BestSpeed, List<ScoreInfoDto> BestAccuracy);
