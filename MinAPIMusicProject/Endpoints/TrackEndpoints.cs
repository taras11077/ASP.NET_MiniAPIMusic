using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;

namespace MinAPIMusicProject.Endpoints;

public static class TrackEndpoints
{
    public static void AddTrackEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/tracks");
        
        
        endpoint.MapGet("/", async (
                MusicContext context,
                [FromQuery]int page = 0, 
                [FromQuery]int size = 10, 
                [FromQuery]string? q = null) =>
            {
                var tracks = q == null ? context.Tracks : context.Tracks.Where(x => x.Title.Contains(q));
                var result = await tracks.Skip(page * size)
                    .Take(size)
                    .ToListAsync();
                
                return Results.Ok(result);
            })
            .WithName("Get tracks endpoint")
            .WithDescription("Get tracks from database...");

        
        
        endpoint.MapGet("{id}", async (MusicContext context, [FromRoute]int id) =>
        {
            var track = await context.Tracks.FindAsync(id);
            if (track == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(track);
        });
        
        
        endpoint.MapPost("{id}/play", async (
            MusicContext context, 
            IMapper mapper,
            [FromRoute] int id, 
            CancellationToken cancellationToken = default) =>
        {
            var track = await context.Tracks.FindAsync(id);

            if (track == null)
            {
                return Results.NotFound();
            }
            
            track.Listened++;
            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(mapper.Map<TrackDTO>(track));
        });
        
        
        endpoint.MapGet("/recommendations", async (
            MusicContext context,
            IMapper mapper,
            [FromQuery] int limit = 10,
            [FromQuery] string genres = "",
            [FromQuery] int minDuration = 1,
            [FromQuery] int maxDuration = int.MaxValue,
            CancellationToken cancellationToken = default) =>
        {
            var genreList = genres.Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            var tracks = await context.Tracks.Where(x =>
                    x.DurationInSeconds >= minDuration && x.DurationInSeconds <= maxDuration &&
                    (!genreList.Any() || genreList.Contains(x.Genre.Name.ToLower())))
                .OrderByDescending(x => x.Listened)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return Results.Ok(mapper.Map<IEnumerable<TrackDTO>>(tracks));
        });
        
    }
}