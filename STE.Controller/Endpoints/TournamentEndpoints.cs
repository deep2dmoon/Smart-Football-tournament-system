namespace smarttournamentengine.STE.Controller.Endpoints;

public static class TournamentEndPoints
{
    public static RouteGroupBuilder MapTournamentEndpoints(this WebApplication app)
    {
        RouteGroupBuilder TournamentGroup = app.MapGroup("/api/tournaments");
        TournamentGroup.MapGet("/", () => "Welcome to tournament endpoints");
        return TournamentGroup;
    }
}