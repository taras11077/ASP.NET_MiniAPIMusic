using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;
using MinAPIMusicProject.Requests;

namespace MinAPIMusicProject.Endpoints;

//[Authorize]
public static class UserEndpoints
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var configuration = app.ServiceProvider.GetRequiredService<IConfiguration>();
        var endpoint = app.MapGroup("/api/users");

        // create (register)
        endpoint.MapPost("register", async (
            IUserService service,
            CreateUserRequest request,
            HttpContext httpContext,
            CancellationToken cancellationToken = default) =>
        {
            var userDb = await service.RegisterUser(request, cancellationToken);
            var jwt = JwtGenerator.GenerateJwt(userDb, configuration.GetValue<string>("TokenKey")!,
                DateTime.UtcNow.AddMinutes(5));

            httpContext.Session.SetInt32("id", userDb.Id);

            return Results.Created("token", jwt);
        });
        
        // login
        endpoint.MapPost("login", async (
            IUserService service,
            CreateUserRequest request,
            CancellationToken cancellationToken = default) =>
        {
            var userDb = service.LoginUser(request, cancellationToken);
            var jwt = JwtGenerator.GenerateJwt(userDb, configuration.GetValue<string>("TokenKey")!,
                DateTime.UtcNow.AddMinutes(5));

            return Results.Created("token", jwt);
        });
        

        // read
        endpoint.MapGet("/", [Authorize] async (
            IUserService service,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10,
            CancellationToken cancellationToken = default) =>
        {
            var result = await service.GetUsers(page, size, cancellationToken);

            return Results.Ok(result);
        });
        
        // update
        endpoint.MapPut("{id}", [Authorize] async (
            IUserService service,
            [FromRoute] int id,
            [FromBody] UserDTO userDto,
            CancellationToken cancellationToken = default) =>
        {
            userDto.Id = id;
            var artistFromDb = await service.UpdateUser(userDto, cancellationToken);

            return Results.Ok(artistFromDb);
        });

        // delete
        endpoint.MapDelete("{id}",[Authorize] async (
            IUserService service,
            [FromRoute] int id,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await service.DeleteUser(id, cancellationToken);

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
        
        
        // просмотр лайкнутих юзером треків
        app.MapGet("/user/{id}/liked-tracks", async (
            IUserService service,
            [FromRoute] int id,
            CancellationToken cancellationToken = default) =>
        {
            var userDb = await service.GetUserWithLikedTracks(id, cancellationToken);

            if (userDb == null)
            {
                return Results.NotFound();
            }

            var likedTracks = userDb.LikedTracks.Select(lt => new 
            {
                lt.Track.Id,
                lt.Track.Title,
                lt.Track.DurationInSeconds,
                lt.Track.Listened,
                lt.Track.CreatedAt
            });

            return Results.Ok(likedTracks);
        });
        
        
        
        
        // лайкнути трек
        app.MapPost("/user/{userId}/like-track/{trackId}", async (
            IUserService userService,
            ITrackService trackService,
            ILikeService likeService,
            int userId, 
            int trackId, 
            CancellationToken cancellationToken = default) =>
        {
            var user = await userService.GetUserById(userId);
            var track = await trackService.GetTrackById(trackId);
        
            if (user == null || track == null)
            {
                return Results.NotFound();
            }

            var existingLike = await likeService.GetLikeByUserAndTrack(userId, trackId);
        
            if (existingLike != null)
            {
                return Results.BadRequest("Track already liked by the user.");
            }
        
            var like = new Like
            {
                UserId = userId,
                TrackId = trackId,
                LikedAt = DateTime.UtcNow
            };
        
            var likeDto = likeService.AddLike(like);
        
            return Results.Created("like", likeDto);
        });
        
        
    }

}