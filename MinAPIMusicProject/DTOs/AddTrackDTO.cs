namespace MinAPIMusicProject.DTOs;

public class AddTrackDTO
{
    public string Title { get; set; }
    public int DurationInSeconds { get; set; }
    
    public int GenreId { get; set; }
    public DateTime? CreatedAt { get; set; }
}