using fastTyping.Common.Interfaces;

namespace fastTyping.Common.Dtos
{
	public sealed class ProgessInfo
	{
		public List<TypistProgess> TypistsProgress { get; } = new();

        protected ProgessInfo()
		{
		}

		public static ProgessInfo CalculateProgess(Room room)
		{
			ProgessInfo progessInfo = new();
			foreach(var typist in room.GetTypists())
			{
				TypistProgess typistProgess = new(
					typist.TypistId, typist.WrittenWords / typist.TotalWords);
                progessInfo.TypistsProgress.Add(typistProgess);
            }
			return progessInfo;
		}
	}
}

