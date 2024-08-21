using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;

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
    }
}