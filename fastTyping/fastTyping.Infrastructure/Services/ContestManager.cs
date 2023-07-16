using fastTyping.Common;
using fastTyping.Common.Dtos;
using fastTyping.Common.Interfaces;
using fastTyping.FastTypingHub;
using Microsoft.AspNetCore.SignalR;

namespace fastTyping.Infrastructure.Services
{
	public class ContestManager : IContestManager
	{
        private Room? _room;
        private DateTime _startAt;
        private readonly IHubContext<TypingHub> _hubContext;

        public ContestManager(IHubContext<TypingHub> hubContext)
		{
            _hubContext = hubContext;
        }


        public void SetRoom(Room room)
        {
            _room ??= (Room)room;
        }

        public async Task<bool> TryStartContestForRoom()
        {
            try
            {
                if (_room is null)
                {
                    throw new NullReferenceException(nameof(ContestManager));
                }
                if (_room.IsFull())
                {
                    _startAt = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    var groupName = _room.RoomId.ToString();
                    await _hubContext.Clients.Groups(groupName).SendAsync("StartContest", _startAt);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[TryStartContestForRoom] EX msg: {ex.Message}, EX stack trace: {ex.StackTrace}");
            }
            return false;
        }

        public Task EndContestForRoom()
        {
            return Task.CompletedTask;
        }

        public async Task NotifyRoomTypistsOfOthersProgess()
        {
            try
            {
                if (_room is null)
                {
                    throw new NullReferenceException(nameof(ContestManager));
                }
                ProgessInfo progessInfo = ProgessInfo.CalculateProgess(_room);
                var groupName = _room.RoomId.ToString();
                await _hubContext.Clients.Groups(groupName).SendAsync("ProgressUpdate", progessInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[NotifyRoomTypistsOfOthersProgess] EX msg: {ex.Message}, EX stack trace: {ex.StackTrace}");
            }
        }
    }
}

