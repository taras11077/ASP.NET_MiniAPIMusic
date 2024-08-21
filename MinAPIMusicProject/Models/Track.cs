namespace MinAPIMusicProject.Models;

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int DurationInSeconds { get; set; }
    public virtual Artist Artist { get; set; }
    public virtual Genre Genre { get; set; }
}