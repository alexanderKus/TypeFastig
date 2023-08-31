using System;
using Application.Interfaces.Services;
using Domain.Models.Dtos;
using Domain.Models.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.SingalR;

[AllowAnonymous]
public class RoomHub : Hub
{
    private readonly IRoomService _roomService;

    public RoomHub(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task JoinRoom(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new HubException("Username cannot be null");
        }
        string connectionId = Context.ConnectionId;
        Player player = new() {
            Name = name,
            ConnectionId = connectionId,
            Progress = 0
        };
        var room = _roomService.JoinRoom(player);
        var gourpName = room.Id.ToString();
        var stats = _roomService.GetStats(room.Id);
        await Groups.AddToGroupAsync(connectionId, gourpName);
        await Clients.Client(connectionId).SendAsync("SetRoomId", room.Id);
        await Clients.Group(gourpName).SendAsync("UpdateStats", stats);
        if (room.IsFull)
        {
            await Clients.Group(gourpName).SendAsync("StartRoom", true);
        }
    }

    public async Task SendStats(Guid roomId, PlayerDto player)
    {
        string connectionId = Context.ConnectionId;
        Player p = new() {
            Name = player.Name,
            ConnectionId = connectionId,
            Progress = player.Progress
        };
        var room = _roomService.UpdateStats(roomId, p);
        if (room is not null)
        {
            var gourpName = room.Id.ToString();
            var stats = _roomService.GetStats(room.Id);
            await Clients.Group(gourpName).SendAsync("UpdateStats", stats);
        }
    }

    public async Task LeaveRoom(Guid roomId)
    {
        string connectionId = Context.ConnectionId;
        var gourpName = roomId.ToString();
        // FIXME: fix this bug
        //_roomService.LeaveRoom(connectionId);
        var stats = _roomService.GetStats(roomId);
        await Clients.Group(gourpName).SendAsync("UpdateStats", stats);
    }
}
