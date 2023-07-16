using fastTyping.Common.Entity;

namespace fastTyping.Tests.Entities
{
	public class TypistTest
	{
		public TypistTest()
		{
		}

        [Fact]
		public void Typist_HasInitValues_Test()
		{
			Typist typist = new();

			Assert.Null(typist.UserId);
			Assert.Null(typist.RoomId);
			Assert.Equal(0, typist.WrittenWords);
			Assert.Equal(0, typist.TotalWords);
			Assert.Equal(0, typist.MadeErrors);
		}
	}
}

