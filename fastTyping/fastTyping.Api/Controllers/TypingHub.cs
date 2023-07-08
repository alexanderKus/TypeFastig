using System;
using Microsoft.AspNetCore.SignalR;

namespace fastTyping.Api.Controllers
{
	public class TypingHub : Hub
	{
		public async Task SendMessage(string user, string msg)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, msg);
		}
        public async Task SendMessage2(string msg)
        {
            await Clients.All.SendAsync("ReceiveMessage", msg);
        }
    }
}

