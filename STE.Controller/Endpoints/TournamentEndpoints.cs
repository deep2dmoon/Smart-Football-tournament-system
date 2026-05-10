using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Application;
using smarttournamentengine.STE.Application.Engine.Components;
using smarttournamentengine.STE.DTOs;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Controller.Endpoints;

public static class TournamentEndPoints
{
    public static RouteGroupBuilder MapTournamentEndpoints(this WebApplication app)
    {
        RouteGroupBuilder TournamentGroup = app.MapGroup("/api/tournaments").WithParameterValidation();
        TournamentGroup.MapGet("/", async (DatabaseContext databaseContext) =>
        databaseContext.Tournaments.Include(t => t.Participants).ToList()
        );

        TournamentGroup.MapPost("/create/{tournamentId}/group/{grounNumber}/participants", async (string tournamentId, TournamentEngine engine, int grounNumber) =>
        {
            Response response = await engine.SpreadToGroups(tournamentId, grounNumber);
            return response.Status switch
            {
                Status.BadRequest => Results.BadRequest(response),
                Status.NotFound => Results.NotFound(response),
                _ => Results.Ok(response)

            };
        });

        TournamentGroup.MapPost("/new/", async (TournamentEngine engine, TournamentDTO tournamentDTO) =>
        {
            Response response = await engine.CreateTournament(tournamentDTO);
            return response.code != 200 ? Results.BadRequest(response) : Results.Ok(response);
        });


        TournamentGroup.MapPost("/{tournamentID}/add/participants/team", async (TournamentEngine engine, string tournamentID, TeamDTO teamDTO) =>
        {
            object response = await engine.AddParticipants(tournamentID, teamDTO);
            return Results.Ok(response);

        }).RequireAuthorization(policy => policy.RequireRole("Executive"));


        TournamentGroup.MapGet("/{tournamentID}/groups", async (string tournamentID, TournamentEngine engine) =>
        {
            object response = await engine.GetTournamentsGroups(tournamentID)!;
            return response switch
            {
                null => Results.BadRequest(response),
                _ => Results.Ok(response)
            };
        });


        TournamentGroup.MapPost("/{tournamentID}/build/fixtures", async (string tournamentID, TournamentEngine engine) =>
        {
            object response = await engine.BuildFixtures(tournamentID);
            return Results.Ok(response);
        });


        TournamentGroup.MapGet("{tournamentID}/match/fixtures", (string tournamentID, DatabaseContext context) =>
        context.Fixtures.Include(f => f.Matches).ToList());

        TournamentGroup.MapPost("/update/standings/{tournamentID}/{fixtureID}", async (TournamentEngine engine, string tournamentID, string fixtureID) =>
        {
            List<Standing> standings = await engine.UpdateStandings(tournamentID, fixtureID);
            return Results.Ok(standings);
        });

        TournamentGroup.MapGet("/{tournamentID}/table", async (TournamentEngine engine, string tournamentID) =>
        {
            List<Standing>? standings = await engine.GetOverallTable(tournamentID)!;
            return standings != null ? Results.Ok(standings) : Results.NoContent();
        });


        TournamentGroup.MapGet("/{tournamentID}/group/{groupID}/table/standings", async (TournamentEngine engine, string groupID, string tournamentID) =>
        {
            List<Standing> standings = engine.GetStandingsInGroup(groupID, tournamentID);
            return Results.Ok(standings);
        });


        TournamentGroup.MapPatch("/{tournamentID}/disqualify", (string tournamentID, TournamentEngine engine) =>
        {
            engine.DisQualifyAndMakeNewParticipants(tournamentID);
        });

        return TournamentGroup;
    }
}