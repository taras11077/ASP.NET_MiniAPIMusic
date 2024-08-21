using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Services;

public class ArtistService : IArtistService
{
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public ArtistService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ArtistDTO> AddArtist(ArtistDTO artist, CancellationToken cancellationToken = default)
    {
        var artistFromDb = _context.Add(_mapper.Map<Artist>(artist));
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ArtistDTO>(artistFromDb); 
    }
    
    public async Task DeleteArtist(int id, CancellationToken cancellationToken = default)
    {
        var artist = await _context.Artists.FindAsync(new object[]{id}, cancellationToken: cancellationToken);

        if (artist == null)
        {
            throw new ArgumentNullException(nameof(artist) + " is null");
        }
        
        _context.Remove(artist);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ArtistDTO> UpdateArtist(ArtistDTO artist, CancellationToken cancellationToken = default)
    {
        var entity = _context.Update(_mapper.Map<Artist>(artist));
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ArtistDTO>(entity.Entity);
    }

    public async Task<TrackDTO> AddTrack(int artistId, int genreId, AddTrackDTO track, CancellationToken cancellationToken = default)
    {
        var trackToAdd = _mapper.Map<Track>(track);
        
        var artist = await _context.Artists.FindAsync(new object[]{artistId}, cancellationToken: cancellationToken);
        if (artist == null)
        {
            throw new ArgumentNullException(nameof(artist) + " is null");
        }
        
        var genre = await _context.Genres.FindAsync(new object[]{genreId}, cancellationToken: cancellationToken);
        if (genre == null)
        {
            throw new ArgumentNullException(nameof(genre) + " is null");
        }

        trackToAdd.Artist = artist;
        trackToAdd.Genre = genre;
        
        _context.Add(trackToAdd);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TrackDTO>(trackToAdd);
    }

    public Task<List<ArtistDTO>> GetArtists(int page, int size, string? q, CancellationToken cancellationToken = default)
    {
        var artists = q == null ? _context.Artists : _context.Artists.Where(x => x.Name.Contains(q));
        return artists.Skip(page * size)
            .Take(size)
            .Select(x => new ArtistDTO() { Name = x.Name })
            .ToListAsync(cancellationToken);
    }
}