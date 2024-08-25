namespace MinAPIMusicProject.Models;

public class Like
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int TrackId { get; set; }
    public virtual Track Track { get; set; }

    public DateTime LikedAt { get; set; }
}