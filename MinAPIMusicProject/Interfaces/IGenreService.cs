using MinAPIMusicProject.DTOs;

namespace MinAPIMusicProject.Interfaces;

public interface IGenreService
{
    Task<GenreDTO> AddGenre(AddGenreDTO genreDto, CancellationToken cancellationToken = default);

    Task DeleteGenre(int id, CancellationToken cancellationToken = default);
   
    Task<List<GenreDTO>> GetGenre(int page, int size,CancellationToken cancellationToken = default);
    
    Task<List<GenreDTO>> SearchGenre(int page, int size, string? q, CancellationToken cancellationToken = default);
}