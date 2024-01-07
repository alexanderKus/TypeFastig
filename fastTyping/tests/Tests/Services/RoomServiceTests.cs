using System;
using Application.Interfaces.Services;
using Infrastructure.Services;
using Moq;
using Tests.Helpers;

namespace Tests.Services
{
    public class RoomServiceTests
    {
        private readonly IRoomService _roomService;

        public RoomServiceTests()
        {
            _roomService = new RoomService();
        }

        [Fact]
        public void Room_Should_BeFull()
        {
            var player1 = PlayerHelper.CreatePlayer(1);
            var player2 = PlayerHelper.CreatePlayer(2);
            var player3 = PlayerHelper.CreatePlayer(3);

            var room = _roomService.JoinRoom(player1);
            _roomService.JoinRoom(player2);
            _roomService.JoinRoom(player3);

            Assert.True(room.IsFull);
        }

        [Fact]
        public void Rooms_Should_BeSame()
        {
            var player1 = PlayerHelper.CreatePlayer(1);
            var player2 = PlayerHelper.CreatePlayer(2);

            var room1 = _roomService.JoinRoom(player1);
            var room2 = _roomService.JoinRoom(player2);

            Assert.Equal(room1, room2);
        }

        [Fact]
        public void Rooms_Should_BeDifferent()
        {
            var player1 = PlayerHelper.CreatePlayer(1);
            var player2 = PlayerHelper.CreatePlayer(2);
            var player3 = PlayerHelper.CreatePlayer(3);
            var player4 = PlayerHelper.CreatePlayer(4);

            var room1 = _roomService.JoinRoom(player1);
            _roomService.JoinRoom(player2);
            _roomService.JoinRoom(player3);
            var room2 = _roomService.JoinRoom(player4);

            Assert.NotEqual(room1, room2);
        }
    }
}

