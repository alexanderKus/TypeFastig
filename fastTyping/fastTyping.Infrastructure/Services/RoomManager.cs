using System.Collections.Concurrent;
using fastTyping.Common;
using fastTyping.Common.Entity;
using fastTyping.Common.Interfaces;
using fastTyping.FastTypingHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace fastTyping.Infrastructure.Services
{
	public class RoomManager : IRoomManager
	{
        private static readonly List<Room> _rooms = new();
        private readonly IServiceProvider _serviceProvider;

        public RoomManager(IServiceProvider serviceProvider)
		{
            _serviceProvider = serviceProvider;
        }

        public Room AddTypistToRomm(Typist typist)
        {
            foreach(var room in _rooms)
            {
                if (!room.IsFull())
                {
                    room.AddTypist(typist);
                    if (room.IsFull())
                    {
                        StartContest(room);
                    }
                    return room;
                }
            }
            Room newRoom = new();
            newRoom.AddTypist(typist);
            _rooms.Add(newRoom);
            return newRoom;
        }

        public void RemoveTypistFromRoom(Guid roomId, Typist typist)
        {
            var room = _rooms.FirstOrDefault(x => x.RoomId == roomId);
            room?.RemoveTypist(typist);
        }

        public void UdpateTypistStats(Guid roomId, Typist typist)
        {
            var room = _rooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room is not null)
            {
                var tt = room.GetTypists().FirstOrDefault(x => x.TypistId == typist.TypistId)
                    ?? throw new NullReferenceException($"Typist with id: {typist.TypistId} is not in room: {roomId}");
                tt.WrittenWords = typist.WrittenWords;
                tt.MadeErrors = typist.MadeErrors;
            }
        }

        private void StartContest(Room room)
        {
            Task.Run(async () =>
            {
                var hubContext = _serviceProvider.GetRequiredService<IHubContext<TypingHub>>();
                var contestManager = new ContestManager(hubContext);
                contestManager.SetRoom(room);
                bool isStarted = await contestManager.TryStartContestForRoom();
                var finishAt = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                if (isStarted)
                {
                    while (finishAt > DateTime.UtcNow)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                        await contestManager.NotifyRoomTypistsOfOthersProgess();
                    }
                }
            });
        }
    }
}

