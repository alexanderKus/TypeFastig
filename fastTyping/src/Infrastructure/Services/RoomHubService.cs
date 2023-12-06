using System;
using System.Text.RegularExpressions;
using Application.Interfaces.Services;
using Domain.Models.Dtos;
using Domain.Models.Room;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Services;

public class RoomHubService : IRoomHubService
{
    private readonly IHubContext<RoomHub> _hub;
    private readonly IRoomService _roomService;

    public RoomHubService(IHubContext<RoomHub> hub, IRoomService roomService)
    {
        _hub = hub;
        _roomService = roomService;
    }

    public async Task JoinRoom(string name, string connectionId)
    {
        Player player = new() {
            Name = name,
            ConnectionId = connectionId,
            Progress = 0
        };
        var room = _roomService.JoinRoom(player);
        var groupName = room.Id.ToString();
        var stats = _roomService.GetStats(room.Id);
        await _hub.Groups.AddToGroupAsync(connectionId, groupName);
        await _hub.Clients.Client(connectionId).SendAsync("SetRoomId", room.Id);
        await _hub.Clients.Client(connectionId).SendAsync("SetLanguage", room.Language);
        await _hub.Clients.Group(groupName).SendAsync("UpdateStats", stats);
        if (room.IsFull)
        {
            await _hub.Clients.Group(groupName).SendAsync("StartRoom", true);
        }
    }

    public async Task LeaveRoom(Guid roomId, string connectionId)
    {
        var groupName = roomId.ToString();
        _roomService.LeaveRoom(connectionId);
        var stats = _roomService.GetStats(roomId);
        await _hub.Clients.Group(groupName).SendAsync("UpdateStats", stats);
    }

    public async Task SendStats(Guid roomId, PlayerDto player, string connectionId)
    {
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
            await _hub.Clients.Group(gourpName).SendAsync("UpdateStats", stats);
        }
    }
}

