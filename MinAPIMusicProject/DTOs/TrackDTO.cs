namespace MinAPIMusicProject.DTOs;

public class TrackDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int DurationInSeconds { get; set; }
    public ArtistDTO Artist { get; set; }
}