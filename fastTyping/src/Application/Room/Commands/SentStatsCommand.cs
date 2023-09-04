using System;
using Domain.Models.Dtos;
using MediatR;

namespace Application.Room.Commands;

public record SentStatsCommand(Guid RoomId, PlayerDto Player, string ConnectionId)
    : IRequest<Unit>;
