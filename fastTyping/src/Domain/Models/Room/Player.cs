namespace Domain.Models.Room;

public class Player
{
    public string Name { get; set; } = default!;
    public string ConnectionId { get; init; } = default!;
    public int Progress { get; set; }
}
