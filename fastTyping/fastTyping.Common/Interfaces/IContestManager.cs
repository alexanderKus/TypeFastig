namespace fastTyping.Common.Interfaces
{
	public interface IContestManager
	{
		public void SetRoom(Room room);
		public Task<bool> TryStartContestForRoom();
		public Task EndContestForRoom();
		public Task NotifyRoomTypistsOfOthersProgess();
	}
}
