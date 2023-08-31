using System;
namespace Domain.Models.Room;

public class Room
{
    private readonly List<Player> _players = new();
    private const int _roomSize = 3;

    public Guid Id { get; } = Guid.NewGuid();
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    public bool IsFull => _players.Count == _roomSize;
    public bool IsEmpty => _players.Count == 0;
    public bool IsStarted { get; set; }

    public void AddPlayer(Player player)
    {
        if (!IsStarted)
        {
            _players.Add(player);
        }
    }

    public void RemovePlayerByConnectionId(string connectionId)
    {
        var player = _players.FirstOrDefault(x => x.ConnectionId == connectionId);
        if (player is not null)
        {
            _players.Remove(player);
        }
    }

    public void UpdatePlayerStats(Player player)
    {
        var p = _players.FirstOrDefault(x => x.ConnectionId == player.ConnectionId);
        if (p is not null)
        {
            p.Progress = player.Progress;
        }
    }

    public bool ContainsPlayerWithConnectionId(string connectionId) 
        => _players.Any(x => x.ConnectionId == connectionId);

    public void StartRoom() => IsStarted = true;
}
