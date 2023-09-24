using System;
using Domain.Models.Room;

namespace Tests.Helpers
{
    public static class PlayerHelper
    {
        public static Player CreatePlayer(int index)
        {
            return new Player {
                Name = $"Player_{index}",
                ConnectionId = Guid.NewGuid().ToString(),
                Progress = 0
            };
        }
    }
}

