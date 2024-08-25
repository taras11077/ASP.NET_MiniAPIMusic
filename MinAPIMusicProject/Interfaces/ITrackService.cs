using MinAPIMusicProject.DTOs;

namespace MinAPIMusicProject.Interfaces;

public interface ITrackService
{
    Task<TrackDTO> GetTrackById(int id, CancellationToken cancellationToken = default);
}