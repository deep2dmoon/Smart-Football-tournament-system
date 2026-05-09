using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.DTOs;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application.Engine.Components;

public class ParticipantOnboardEngine(DatabaseContext context, ILogger<ParticipantOnboardEngine> logger_)
{
    private ILogger<ParticipantOnboardEngine> logger = logger_;

    private readonly DatabaseContext databaseContext = context;
    public async Task<object> AddParticipants(string tournamentID, TeamDTO teamDTO)
    {
        Tournament? tournament = await databaseContext.Tournaments
        .Include(t => t.Participants)
        .FirstOrDefaultAsync(t => t.TournamentID == tournamentID);
        try
        {
            if (tournament == null)
            {
                return new { response = "No tournament record found" };
            }
            Team team = new() { Name = teamDTO.TeamName, TeamID = Guid.NewGuid().ToString() };
            Team? existingTeam = tournament.Participants.FirstOrDefault(p => p.Name == teamDTO.TeamName);
            if (existingTeam != null)
                return new { response = "Team Name exist in the tournament" };
            tournament!.Participants.Add(team);
            await databaseContext.SaveChangesAsync();
            return new { response = $"New team  added to the Tournament" };
        }
        catch (System.Exception)
        {
            logger.LogError("error adding new team");
            throw;
        }
    }
}