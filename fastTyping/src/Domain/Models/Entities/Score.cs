namespace Domain.Models.Entities;

public class Score
{
    public int Id { get; set; }
    public TimeSpan Time { get; set; }
    public float Precision { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
