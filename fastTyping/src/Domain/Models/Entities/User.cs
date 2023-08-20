namespace Domain.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;

    public ICollection<Score> Scores { get; } = new List<Score>();
}
