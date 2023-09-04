using System;
using MediatR;

namespace Application.Room.Commands;

public record JoinRoomCommand(string Name, string ConnectionId)
    : IRequest<Unit>;
