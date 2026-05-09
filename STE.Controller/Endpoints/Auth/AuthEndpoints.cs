using smarttournamentengine.STE.DTOs;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.Services;

namespace smarttournamentengine.STE.Controller.Endpoints.Auth;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        RouteGroupBuilder authGroup = app.MapGroup("/api/auth").WithParameterValidation();

        authGroup.MapPost("/login", async (AuthService authService, UserDTO userDTO) =>
        {
            User? user = await authService.ValidateUser(userDTO)!;
            return user switch
            {
                null => Results.BadRequest(new { response = "Invalid user credentials" }),
                _ => Results.Ok(new { token = authService.GenerateJwttoken(user), user }),
            };
        });
        return authGroup;
    }
}