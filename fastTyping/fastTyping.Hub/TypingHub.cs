using fastTyping.Common.Entity;
using fastTyping.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace fastTyping.Hub
{
    public class TypingHub : Hub
    {
        private readonly IRoomManager _roomManager;

        public TypingHub(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} has connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} has disconnected");
        }

        public async Task JoinRoom(Typist typist)
        {
            try
            {
                if (typist.WrittenWords == 0)
                {
                    return;
                }
                var roomId = _roomManager.AddTypistToRomm(typist);
                var groupName = roomId.ToString();
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName)
                    .SendAsync("Send", $"{typist.TypistId} has joined the room: {roomId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[JoinRoom] EX msg: {ex.Message}, EX stack trace: {ex.StackTrace}");
            }

        }

        public async Task LeaveRoom(Typist typist)
        {
            try
            {
                var groupName = typist.RoomId.ToString()
                    ?? throw new ArgumentNullException();
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName)
                    .SendAsync("Send", $"{typist.TypistId} has left the room");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[LeaveRoom] EX msg: {ex.Message}, EX stack trace: {ex.StackTrace}");
            }
        }
    }
}

