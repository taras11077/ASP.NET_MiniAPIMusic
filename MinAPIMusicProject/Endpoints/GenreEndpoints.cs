using Microsoft.AspNetCore.Mvc;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;

namespace MinAPIMusicProject.Endpoints;

public static class GenreEndpoints
{
     public static void AddGenreEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/genres");
        
        // get all
        endpoint.MapGet("/", async (
            IGenreService service,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10,
            CancellationToken cancellationToken = default) =>
        {
            var genres = await service.GetGenre(page, size, cancellationToken);

            return Results.Ok(genres);
        });
        
        // search
        endpoint.MapGet("{q}", async (
            IGenreService service,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10,
            [FromRoute] string? q = null,
            CancellationToken cancellationToken = default) =>
        {
            var genres = await service.SearchGenre(page, size, q, cancellationToken);

            return Results.Ok(genres);
        });
        
        // add
        endpoint.MapPost("/", async (
            IGenreService service,
            GenreDTO genre,
            CancellationToken cancellationToken = default) =>
        {
            var genreFromDb = await service.AddGenre(genre, cancellationToken);

            return Results.Created($"create", genreFromDb.Id);
        });
        
        // remove
        endpoint.MapDelete("{id}", async (
            IGenreService service, 
            [FromRoute]int id,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await service.DeleteGenre(id, cancellationToken);

                return Results.Ok();
            }
            catch (ArgumentNullException)
            {
                return Results.NotFound();
            }
            catch (Exception ex) 
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}