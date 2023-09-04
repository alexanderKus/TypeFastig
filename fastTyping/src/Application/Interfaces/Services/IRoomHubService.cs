using System;
using Domain.Models.Dtos;

namespace Application.Interfaces.Services;

public interface IRoomHubService
{
    Task JoinRoom(string name, string connectionId);
    Task SendStats(Guid roomId, PlayerDto player, string connectionId);
    Task LeaveRoom(Guid roomId, string connectionId);
}
