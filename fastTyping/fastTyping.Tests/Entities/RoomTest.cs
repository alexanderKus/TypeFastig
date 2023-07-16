using fastTyping.Common;
using fastTyping.Common.Entity;

namespace fastTyping.Tests.Entities
{
	public class RoomTest
	{
		public RoomTest()
		{
		}

		[Fact]
		public void Room_HasInitValues_Test()
		{
			Room room = new();

			Assert.NotNull(room.GetTypists());
			Assert.Empty(room.GetTypists());
		}

		[Fact]
        public void Room_ThrowExceptionOnRemoveTypist_Test()
		{
            Room room = new();
			Typist typist = new();

			Assert.Throws<InvalidOperationException>(() => room.RemoveTypist(typist));
        }

        [Fact]
        public void Room_ThrowExceptionOnAdd6thTypist_Test()
        {
            Room room = new();
            Typist typist1 = new();
            Typist typist2 = new();
            Typist typist3 = new();
            Typist typist4 = new();
            Typist typist5 = new();
            Typist typist6 = new();

            room.AddTypist(typist1);
            room.AddTypist(typist2);
            room.AddTypist(typist3);
            room.AddTypist(typist4);
            room.AddTypist(typist5);

            Assert.Throws<InvalidOperationException>(() => room.AddTypist(typist6));
        }
    }
}

