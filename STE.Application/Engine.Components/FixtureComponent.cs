using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application.Engine.Components;

public class FixtureEngine(DatabaseContext context)
{
    private readonly DatabaseContext databaseContext = context;

    public async Task<object> BuildFixtures(string tournamentID)
    {

        foreach (Fixtures fixture in databaseContext.Fixtures.Include(g => g.Matches))
        {
            if (fixture.TournamentID == tournamentID) databaseContext.Fixtures.Remove(fixture);
            databaseContext.Fixtures.Remove(fixture);

        }


        Fixtures fixtures = new() { ID = Guid.NewGuid().ToString("N"), TournamentID = tournamentID };

        foreach (Group group in databaseContext.Groups.Include(g => g.Teams))
        {
            if (group.TournamentID == tournamentID)
            {
                for (int i = 0; i < group.Teams.Count; i++)
                {
                    for (int j = i + 1; j < group.Teams.Count; j++)
                    {
                        string home;
                        string away;
                        Random random = new();
                        if (i + j % 2 == 0)
                        {
                            home = group.Teams[i].Name;
                            away = group.Teams[j].Name;
                        }
                        else
                        {
                            home = group.Teams[j].Name;
                            away = group.Teams[i].Name;
                        }

                        Match match = new()
                        {
                            MatchID = Guid.NewGuid().ToString(),
                            HomeTeam = home,
                            AwayTeam = away,
                            AwayScore = random.Next(0, 6),
                            HomeScore = random.Next(0, 6)
                        };
                        group.Matches.Add(match);
                        fixtures.Matches.Add(match);
                    }
                }
            }
        }
        databaseContext.Fixtures.Add(fixtures);

        await databaseContext.SaveChangesAsync();
        return new { fixtures };
    }

}
