namespace MinAPIMusicProject.Models;

public class Playlist
{
    public int Id { get; set; }
    public string Title { get; set; }
    public virtual ICollection<Track> Tracks { get; set; }
    public virtual User User { get; set; }
}