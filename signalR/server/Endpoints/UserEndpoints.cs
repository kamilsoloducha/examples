using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Server.Dto;

namespace Server.Endpoints;

public static class UserEndpoints
{
    public static WebApplication AddAuthenticate(this WebApplication app)
    {
        app.MapPost("user/authenticate",
            async (AuthenticateRequest request, IUserService userService, CancellationToken cancellationToken) =>
            {
                var response = await userService.AuthenticateAsync(request, cancellationToken);

                return response == null
                    ? Results.BadRequest(new { message = "Username or password is incorrect" })
                    : Results.Ok(response);
            });
        return app;
    }

    public static WebApplication AddGetUser(this WebApplication app)
    {
        app.MapGet("user", (IUserService userService) =>
        {
            var response = userService.Users.Select(x => new UserResponse
            {
                Id = x.Id,
                Name = x.Name
            });
            return Results.Ok(response);
        });
        return app;
    }
}