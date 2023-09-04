using Application.Room.Commands;
using Domain.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR;

[AllowAnonymous]
public class RoomHub : Hub
{
    private readonly IMediator _mediator;

    public RoomHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task JoinRoom(string name)
    {
        JoinRoomCommand joinRoomCommand = new(name, Context.ConnectionId);
        await _mediator.Send(joinRoomCommand);
    }

    public async Task SendStats(Guid roomId, PlayerDto player)
    {
        SentStatsCommand sentStatsCommand = new(roomId, player, Context.ConnectionId);
        await _mediator.Send(sentStatsCommand);
    }

    public async Task LeaveRoom(Guid roomId)
    {
        LeaveRoomCommand leaveRoomCommand = new(roomId, Context.ConnectionId);
        await _mediator.Send(leaveRoomCommand);
    }
}

