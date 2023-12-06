using Domain.Models.Enums;

namespace Domain.Models.Entities;

public class Score
{
    public int Id { get; set; }
    public int Speed { get; set; }
    public float Accuracy { get; set; }
    public Language Language { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
