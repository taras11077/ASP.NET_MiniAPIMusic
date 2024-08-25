namespace MinAPIMusicProject.DTOs;

public class PlaylistDTO
{
    public string Title { get; set; }
    public IEnumerable<TrackDTO> Tracks { get; set; }
}