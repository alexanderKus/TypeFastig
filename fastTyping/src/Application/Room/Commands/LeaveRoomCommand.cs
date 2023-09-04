using System;
using MediatR;

namespace Application.Room.Commands;

public record LeaveRoomCommand(Guid RoomId, string ConnectionId)
    : IRequest<Unit>;
