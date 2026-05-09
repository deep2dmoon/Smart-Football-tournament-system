using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Application.Engine.Components;
using smarttournamentengine.STE.DTOs;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;
namespace smarttournamentengine.STE.Application;

public class TournamentEngine(DatabaseContext context_, GroupEngine groupEngine, FixtureEngine fixturEngine_, ParticipantOnboardEngine participantOnboardEngine_, ILogger<TournamentEngine> logger_, StandingUpdateComponent standingUpdateComponent_)
{
    private readonly ILogger<TournamentEngine> logger = logger_;
    private readonly FixtureEngine fixtureEngine = fixturEngine_;
    private readonly DatabaseContext databaseContext = context_;
    private readonly GroupEngine groupEngine = groupEngine;
    private readonly ParticipantOnboardEngine participantOnboardEngine = participantOnboardEngine_;
    private readonly StandingUpdateComponent standingUpdateComponent = standingUpdateComponent_;

    public async Task<Response> CreateTournament(TournamentDTO tournamentDTO)
    {
        Tournament tournament = new() { TournamentID = Guid.NewGuid().ToString(), Name = tournamentDTO.Name };
        foreach (Tournament tournament_ in databaseContext.Tournaments)
        {
            if (tournament_.Name == tournamentDTO.Name)
            {
                return new Response { Message = "Tournament name already exist" };
            }
        }

        databaseContext.Tournaments.Add(tournament);
        await databaseContext.SaveChangesAsync();
        return new Response { Message = $"New Tournament {tournament.Name}, ID {tournament.TournamentID} Created", code = 200, Status = Status.Ok };
    }
    public async Task<object> AddParticipants(string tournamentID, TeamDTO teamDTO)
    {
        object response = await participantOnboardEngine.AddParticipants(tournamentID, teamDTO);
        return response;
    }

    public async Task<Response> SpreadToGroups(string tournamentID, int peerNumber)
    {
        Response response = await groupEngine.GroupTeams(tournamentID, peerNumber)!;
        return response;
    }
    public async Task<object> BuildFixtures(string tournamentID)
    {
        object response = await fixtureEngine.BuildFixtures(tournamentID);
        return response;
    }

    public async Task<List<Group>>? GetTournamentsGroups(string tournamentID)
    {
        List<Group>? tournamentGroups = [.. databaseContext.Groups.Include(t => t.Teams)
        .Include(g=>g.Matches)
         .Where(g => g.TournamentID == tournamentID)];
        return tournamentGroups;
    }

    public async Task<List<Standing>> UpdateStandings(string tournamentID, string fixtureID)
    {
        List<Standing>? standings = await standingUpdateComponent.UpdateStandings(tournamentID, fixtureID)!;
        return standings;
    }

    public async Task<List<Standing>>? GetOverallTable(string ID)
    {
        List<StandingsTable> standings = [.. databaseContext.StandingsTables.Include(x => x.Standings)];
        foreach (StandingsTable standingsTable in standings)
        {
            if (standingsTable.TournamentID == ID)
                return standingsTable.Standings.OrderByDescending(x => x.Points).ToList();
        }
        return null!;
    }


    public List<Standing> GetStandingsInGroup(string groupId, string tournamentId)
    {
        List<Standing> standings = groupEngine.GetGroupTableStandings(groupId, tournamentId);
        return standings;
    }

}