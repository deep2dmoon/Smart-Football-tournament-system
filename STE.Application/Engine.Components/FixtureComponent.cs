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


        foreach (Group group in databaseContext.Groups.Include(g => g.Teams).Include(g => g.Matches))
        {

            if (group.TournamentID == tournamentID)
            {
                group.Matches.Clear(); // remove every existing game fixtures 

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

                        Match homeMatch = new()
                        {
                            MatchID = Guid.NewGuid().ToString(),
                            HomeTeam = home,
                            AwayTeam = away,
                            AwayScore = random.Next(0, 6),
                            HomeScore = random.Next(0, 6)
                        };

                        Match awayMatch = new()
                        {
                            MatchID = Guid.NewGuid().ToString(),
                            HomeTeam = away,
                            AwayTeam = home,
                            AwayScore = random.Next(0, 6),
                            HomeScore = random.Next(0, 6)
                        };
                     
                        group.Matches.AddRange([homeMatch, awayMatch]);
                        // logger.LogInformation("group matches is {count}", group.Matches.Count);
                        fixtures.Matches.AddRange([homeMatch, awayMatch]);
                    }
                }
            }
        }
        databaseContext.Fixtures.Add(fixtures);
        await databaseContext.SaveChangesAsync();
        return new { fixtures };
    }

}
