using System;
using Application.Interfaces.Services;
using Domain.Models.Dtos;
using Domain.Models.Room;

namespace Infrastructure.Services;

public class RoomService : IRoomService
{
    private static readonly List<Room> _rooms = new();
    private static object _lock = new();

    public RoomService()
    {
    }

    public IEnumerable<PlayerDto> GetStats(Guid roomId)
    {
        var room = _rooms.FirstOrDefault(x => x.Id == roomId);
        if (room is null)
        {
            return new List<PlayerDto>();
        }
        return room.Players.Select(x => new PlayerDto(x.Name, x.Progress));

    }

    public Room JoinRoom(Player player)
    {
        lock (_lock)
        {
            var room = _rooms.FirstOrDefault(x => !x.IsFull);
            if (room is null)
            {
                Room newRoom = new();
                newRoom.AddPlayer(player);
                _rooms.Add(newRoom);
                return newRoom;
            }
            room.AddPlayer(player);
            return room;
        }
    }

    public Room? LeaveRoom(string userConnectionId)
    {
        lock (_lock)
        {
            var room = _rooms.FirstOrDefault(
                x => x.ContainsPlayerWithConnectionId(userConnectionId));
            room?.RemovePlayerByConnectionId(userConnectionId);
            if (room is not null && room.IsEmpty)
            {
                _rooms.Remove(room);
                return null;
            }
            return room;
        }
    }

    public Room? UpdateStats(Guid roomId, Player player)
    {
        var room = _rooms.FirstOrDefault(x => x.Id == roomId);
        room?.UpdatePlayerStats(player);
        return room;
    }
}
