using System;
using Domain.Models.Dtos;
using Domain.Models.Room;

namespace Application.Interfaces.Services;

public interface IRoomService
{
    Room JoinRoom(Player player);
    Room? LeaveRoom(string userConnectionId);
    Room? UpdateStats(Guid roomId, Player player);
    IEnumerable<PlayerDto> GetStats(Guid roomId);
}
