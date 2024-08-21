using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Services;

public class GenreService : IGenreService
{
    
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public GenreService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<GenreDTO> AddGenre(GenreDTO genreDto, CancellationToken cancellationToken = default)
    {
        var genreFromDb = _context.Add(_mapper.Map<Genre>(genreDto));
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GenreDTO>(genreFromDb); 
    }
    

    public async Task DeleteGenre(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _context.Genres.FindAsync(new object[]{id}, cancellationToken: cancellationToken);

        if (genre == null)
        {
            throw new ArgumentNullException(nameof(genre) + " is null");
        }
        
        _context.Remove(genre);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<List<GenreDTO>> GetGenre(int page, int size,CancellationToken cancellationToken = default)
    {
        return _context.Genres.Skip(page * size)
            .Take(size)
            .Select(x => new GenreDTO() { Name = x.Name })
            .ToListAsync(cancellationToken);
    }

    public Task<List<GenreDTO>> SearchGenre(int page, int size, string? q, CancellationToken cancellationToken = default)
    {
        var genres = q == null ? _context.Genres : _context.Genres.Where(x => x.Name.Contains(q));
        return genres.Skip(page * size)
            .Take(size)
            .Select(x => new GenreDTO() { Name = x.Name })
            .ToListAsync(cancellationToken);
    }
}