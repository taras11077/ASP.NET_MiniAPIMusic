namespace MinAPIMusicProject.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } 
    public virtual ICollection<Like> LikedTracks { get; set; }
}