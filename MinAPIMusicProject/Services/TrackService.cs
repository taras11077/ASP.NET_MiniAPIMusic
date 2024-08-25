using AutoMapper;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;

namespace MinAPIMusicProject.Services;

public class TrackService : ITrackService
{
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public TrackService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    public async Task<TrackDTO> GetTrackById(int id, CancellationToken cancellationToken = default)
    {
        var userFromDb = await _context.Tracks.FindAsync(new object[]{id}, cancellationToken: cancellationToken);
        return _mapper.Map<TrackDTO>(userFromDb);
    }
}