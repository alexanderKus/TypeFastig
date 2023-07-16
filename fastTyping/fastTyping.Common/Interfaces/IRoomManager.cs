using fastTyping.Common.Entity;

namespace fastTyping.Common.Interfaces
{
	public interface IRoomManager
	{
		public Room AddTypistToRomm(Typist typist);
		public void RemoveTypistFromRoom(Guid roomId, Typist typist);
		public void UdpateTypistStats(Guid roomId, Typist typist);
	}
}
