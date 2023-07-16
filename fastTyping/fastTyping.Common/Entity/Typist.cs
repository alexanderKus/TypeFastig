using System;
namespace fastTyping.Common.Entity
{
	public class Typist
	{
		public Guid TypistId { get; set; } = Guid.NewGuid();
		public Guid? UserId { get; init; } = default!;
		public Guid? RoomId { get; set; } = default!;
		public DateTime? StartAt { get; set; } = default!;
		public int WrittenWords { get; set; } = default;
		public int TotalWords { get; set; } = default;
		public int MadeErrors { get; set; } = default;
	}
}

