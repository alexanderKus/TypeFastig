using System;
using System.Text.RegularExpressions;
using Application.Interfaces.Services;
using Domain.Models.Dtos;
using Domain.Models.Room;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Services;

public class RoomHubService : IRoomHubService
{
    private readonly IHubContext<RoomHub> _hub;
    private readonly IRoomService _roomService;
    private readonly ILogger _logger;

    public RoomHubService(IHubContext<RoomHub> hub, IRoomService roomService, ILogger<IRoomHubService> logger)
    {
        _hub = hub;
        _roomService = roomService;
        _logger = logger;
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
        _logger.LogInformation($"Player: {player.Name} with connectionId: {player.ConnectionId} joined to group: {groupName}");
        await _hub.Groups.AddToGroupAsync(connectionId, groupName);
        await _hub.Clients.Client(connectionId).SendAsync("SetRoomId", room.Id);
        await _hub.Clients.Client(connectionId).SendAsync("SetLanguage", room.Language);
        await _hub.Clients.Group(groupName).SendAsync("UpdateStats", stats);
        if (room.IsFull)
        {
            _logger.LogInformation($"Room: {room.Id} has been started");
            await _hub.Clients.Group(groupName).SendAsync("StartRoom", true);
        }
        else 
        {
            await _hub.Clients.Group(groupName).SendAsync("StartRoom", false);
        }
    }

    public async Task LeaveRoom(Guid roomId, string connectionId)
    {
        var groupName = roomId.ToString();
        _roomService.LeaveRoom(connectionId);
        _logger.LogInformation($"Player with connectionId: {connectionId} left to group: {groupName}");
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
            var groupName = room.Id.ToString();
            var stats = _roomService.GetStats(room.Id);
            await _hub.Clients.Group(groupName).SendAsync("UpdateStats", stats);
        }
    }
}

