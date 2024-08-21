using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;

namespace MinAPIMusicProject.Endpoints;

public static class ArtistEndpoints
{
    public static void AddArtistEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/artists");

        endpoint.MapPost("/", async (
            IArtistService service,
            ArtistDTO artist,
            CancellationToken cancellationToken = default) =>
        {
            // validation
            var artistFromDb = await service.AddArtist(artist, cancellationToken);

            return Results.Created($"c", artistFromDb.Id);
        });

        endpoint.MapGet("/", async (
            IArtistService service,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10,
            [FromQuery] string? q = null,
            CancellationToken cancellationToken = default) =>
        {
            var result = await service.GetArtists(page, size, q, cancellationToken);

            return Results.Ok(result);
        });
        
        endpoint.MapDelete("{id}", async (
            IArtistService service, 
            [FromRoute]int id,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await service.DeleteArtist(id, cancellationToken);

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

        endpoint.MapPut("{id}", async (
            IArtistService service,
            [FromRoute] int id,
            [FromBody] ArtistDTO artist,
            CancellationToken cancellationToken = default) =>
        {
            artist.Id = id;
            var artistFromDb = await service.UpdateArtist(artist, cancellationToken);

            return Results.Ok(artistFromDb);
        });
        
        endpoint.MapPost("{artistId}/tracks", async (
            IArtistService service,
            [FromRoute]int artistId,
            [FromQuery]int genreId,
            [FromBody]AddTrackDTO track,
            CancellationToken cancellationToken = default) =>
        {
            var trackFromDb = await service.AddTrack(artistId, genreId, track, cancellationToken);

            return Results.Created($"/api/tracks/{trackFromDb.Id}", trackFromDb.Id);
        });
        
        endpoint.MapGet("{artistId}/tracks", async (
            MusicContext context,
            IMapper mapper,
            [FromRoute]int artistId) =>
        {
            var artist = await context.Artists.FindAsync(artistId);

            return Results.Ok(mapper.Map<IEnumerable<TrackDTO>>(artist.Tracks));
        });
    }
}