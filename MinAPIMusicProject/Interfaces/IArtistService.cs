using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Interfaces;

public interface IArtistService
{
    Task<ArtistDTO> AddArtist(ArtistDTO artist, CancellationToken cancellationToken = default);
    /// <summary>
    /// delete artist from database
    /// </summary>
    /// <param name="id">ID property of artist</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">throws when artist with id is not found</exception>
    Task DeleteArtist(int id, CancellationToken cancellationToken = default);
    Task<ArtistDTO> UpdateArtist(ArtistDTO artist, CancellationToken cancellationToken = default);
    Task<TrackDTO> AddTrack(int artistId, int genreId, AddTrackDTO track, CancellationToken cancellationToken = default);
    
    Task<List<ArtistDTO>> GetArtists(int page, int size, string? q, CancellationToken cancellationToken = default);
}