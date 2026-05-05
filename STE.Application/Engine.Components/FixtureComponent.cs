using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application.Engine.Components;

public class FixtureEngine(DatabaseContext context)
{
    private readonly DatabaseContext databaseContext = context;
    public async Task<object> BuildFixtures(string tournamentID)
    {
        Random random = new();
        Fixtures fixtures_ = new() { FixtureId = Guid.NewGuid().ToString("N") };

        Tournament? tournament = await databaseContext.Tournaments.FirstOrDefaultAsync(t => t.TournamentID == tournamentID);
        if (tournament != null)
        {
            foreach (Group group in tournament.Groups)
            {
                for (int i = 0; i < group.Teams.Count; i++)
                {
                    for (int j = i + 1; j < group.Teams.Count; j++)
                    {
                        string home;
                        string away;
                        if (i + j % 2 == 0)
                        {
                            home = group.Teams[i].Name;
                            away = group.Teams[j].Name;
                        }
                        else
                        {
                            home = group.Teams[i].Name;
                            away = group.Teams[j].Name;
                        }
                        Match match = new()
                        {
                            MatchID = Guid.NewGuid().ToString("N"),
                            HomeTeam = home,
                            AwayTeam = away,
                        };
                        fixtures_.Matches!.Add(match);
                    }
                }
            }

        }
        tournament!.Fixtures.Add(fixtures_);
        await databaseContext.SaveChangesAsync();
        return new { fixtures = fixtures_ };
    }
}