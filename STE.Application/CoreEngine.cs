using smarttournamentengine.STE.Application.Engine.Components;
using smarttournamentengine.STE.DTOs;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application;

public class TournamentEngine(DatabaseContext context_, GroupEngine groupEngine, FixtureEngine fixturEngine_)
{
    private readonly FixtureEngine fixtureEngine = fixturEngine_;
    private readonly DatabaseContext databaseContext = context_;
    private readonly GroupEngine groupEngine = groupEngine;

    public async Task CreateTournament(TournamentDTO tournamentDTO)
    {
        Tournament tournament = new() { TournamentID = Guid.NewGuid().ToString(), Name = tournamentDTO.Name };
        databaseContext.Tournaments.Add(tournament);
        await databaseContext.SaveChangesAsync();
    }
    public object AddTeams(string tournamentID, TeamDTO teamDTO)
    {
        Tournament? tournament = databaseContext.Tournaments.FirstOrDefault(t => t.TournamentID == tournamentID);
        if (tournament != null)
        {
            Team team = new() { TeamID = Guid.NewGuid().ToString(), Name = teamDTO.TeamName };
            tournament.Participants.Add(team);
            databaseContext.SaveChangesAsync();
            return new { response = "New Team Added to the tournament" };
        }
        return new { response = "Tournament not found" };
    }
    public async Task<object> SpreadToGroups(string tournamentID, int groupNumber)
    {
        object response = await groupEngine.GroupTeams(tournamentID, groupNumber);
        return response;
    }
    public async Task<object> BuildFixtures(string tournamentID)
    {
        object resposne = await fixtureEngine.BuildFixtures(tournamentID);
        return resposne;
    }
    public void UpdateStandings() { }
}