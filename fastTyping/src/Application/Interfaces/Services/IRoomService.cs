using Domain.Models.Dtos;
using RoomModel = Domain.Models.Room;

namespace Application.Interfaces.Services;

public interface IRoomService
{
    RoomModel.Room JoinRoom(RoomModel.Player player);
    RoomModel.Room? LeaveRoom(string userConnectionId);
    RoomModel.Room? UpdateStats(Guid roomId, RoomModel.Player player);
    IEnumerable<PlayerDto> GetStats(Guid roomId);
}
