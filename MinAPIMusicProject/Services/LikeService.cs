using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Services;

public class LikeService : ILikeService
{
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public LikeService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LikeDTO> AddLike(Like like, CancellationToken cancellationToken = default)
    {
        _context.Likes.Add(like);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<LikeDTO>(like);
    }

    public async Task<LikeDTO> GetLikeByUserAndTrack(int userId, int trackId, CancellationToken cancellationToken = default)
    {
        var like = await _context.Likes.Where(l => l.UserId == userId && l.TrackId == trackId)
            .FirstOrDefaultAsync(cancellationToken);
        
        return _mapper.Map<LikeDTO>(like);
    }
}