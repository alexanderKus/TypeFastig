using System;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Room.Commands;

public class JoinRoomCommandHandler
    : IRequestHandler<JoinRoomCommand, Unit>
{
    private readonly IRoomHubService _roomHubService;

    public JoinRoomCommandHandler(IRoomHubService roomHubService)
    {
        _roomHubService = roomHubService;
    }

    public async Task<Unit> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        await _roomHubService.JoinRoom(request.Name, request.ConnectionId);
        return Unit.Value;
    }
}
