using fastTyping.Common;
using fastTyping.Common.Entity;
using fastTyping.Common.Interfaces;

namespace fastTyping.Infrastructure.Services
{
	public class RoomManager : IRoomManager
	{
        private readonly List<Room> _rooms = new();

		public RoomManager()
		{
		}

        public Room AddTypistToRomm(Typist typist)
        {
            foreach(var room in _rooms)
            {
                if (!room.IsFull())
                {
                    room.AddTypist(typist);
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
    }
}

