using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Interfaces;

public interface ILikeService
{
    Task<LikeDTO> AddLike(Like like, CancellationToken cancellationToken = default);
    
    Task<LikeDTO> GetLikeByUserAndTrack(int userId, int trackId, CancellationToken cancellationToken = default);
}