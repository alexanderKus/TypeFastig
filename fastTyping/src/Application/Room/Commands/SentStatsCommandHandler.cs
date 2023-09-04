using System;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Room.Commands;

public class SentStatsCommandHandler
    : IRequestHandler<SentStatsCommand, Unit>
{
    private readonly IRoomHubService _roomHubService;

    public SentStatsCommandHandler(IRoomHubService roomHubService)
    {
        _roomHubService = roomHubService;
    }

    public async Task<Unit> Handle(SentStatsCommand request, CancellationToken cancellationToken)
    {
        await _roomHubService.SendStats(
            request.RoomId, request.Player, request.ConnectionId);
        return Unit.Value;
    }
}

