using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Requests;

namespace MinAPIMusicProject.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this IEndpointRouteBuilder app)
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
    }

}