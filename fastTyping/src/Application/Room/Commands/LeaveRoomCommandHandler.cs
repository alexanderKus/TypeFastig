using System;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Room.Commands;

public class LeaveRoomCommandHandler
    : IRequestHandler<LeaveRoomCommand, Unit>
{
    private readonly IRoomHubService _roomHubService;

    public LeaveRoomCommandHandler(IRoomHubService roomHubService)
    {
        _roomHubService = roomHubService;
    }

    public async Task<Unit> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
    {
        await _roomHubService.LeaveRoom(request.RoomId, request.ConnectionId);
        return Unit.Value;
    }
}
