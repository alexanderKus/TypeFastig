using System;
using Domain.Models.Dtos;
using MediatR;

namespace Application.Scores.Queries;

public record Top100ScoresQuery() : IRequest<Top100ScoresDto>;
