using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application.Engine.Components;

public class StandingUpdateComponent(DatabaseContext databaseContext, ILogger<StandingUpdateComponent> logger)
{

    public async Task<List<Standing>>? UpdateStandings(string tournamentID, string fixtureID)
    {

        Dictionary<string, Standing> standings = [];
        logger.LogInformation("total fixture is {total}", databaseContext.Fixtures.Count());
        foreach (Fixtures fixtures in databaseContext.Fixtures)
        {
            if (fixtures.TournamentID == tournamentID)
            {
                databaseContext.Fixtures.Remove(fixtures);
            }
        }
        foreach (Fixtures fixtures in databaseContext.Fixtures.Include(t => t.Matches).ToList())
        {
            // logger.LogInformation("random info {property} {fixtureID} {fixtureArg}", fixtures.TournamentID, fixtures.ID, fixtureID);
            if (fixtures.ID == fixtureID)
                foreach (Match match in fixtures.Matches)
                {

                    if (!standings.ContainsKey(match.HomeTeam))
                    {
                        standings[match.HomeTeam] = new Standing { ID = Guid.NewGuid().ToString(), TeamName = match.HomeTeam };
                    }
                    if (!standings.ContainsKey(match.AwayTeam))
                    {
                        standings[match.AwayTeam] = new Standing { ID = Guid.NewGuid().ToString(), TeamName = match.AwayTeam };
                    }

                    if (match.HomeScore > match.AwayScore)
                    {
                        standings[match.HomeTeam].Points += 3;
                        standings[match.HomeTeam].GoalsScore += match.HomeScore;
                        standings[match.HomeTeam].GoalsConceded += match.AwayScore;
                        standings[match.AwayTeam].GoalsConceded += match.HomeScore;
                        standings[match.AwayTeam].GoalsScore += match.AwayScore;
                        standings[match.HomeTeam].GamePlayed++;
                        standings[match.AwayTeam].GamePlayed++;
                        standings[match.HomeTeam].TeamName = match.HomeTeam;
                        standings[match.AwayTeam].TeamName = match.AwayTeam;
                        standings[match.HomeTeam].GoalsDifference = standings[match.HomeTeam].GoalsScore - standings[match.HomeTeam].GoalsConceded;
                        standings[match.AwayTeam].GoalsDifference = standings[match.AwayTeam].GoalsScore - standings[match.AwayTeam].GoalsConceded;
                    }
                    else if (match.AwayScore == match.HomeScore)
                    {
                        standings[match.HomeTeam].Points += 1;
                        standings[match.AwayTeam].Points += 1;

                        standings[match.HomeTeam].GoalsScore += match.HomeScore;
                        standings[match.AwayTeam].GoalsScore += match.AwayScore;

                        standings[match.AwayTeam].GoalsConceded += match.HomeScore;
                        standings[match.HomeTeam].GoalsConceded += match.AwayScore;

                        standings[match.HomeTeam].GamePlayed++;
                        standings[match.AwayTeam].GamePlayed++;

                        standings[match.HomeTeam].TeamName = match.HomeTeam;
                        standings[match.AwayTeam].TeamName = match.AwayTeam;

                        standings[match.HomeTeam].GoalsDifference = standings[match.HomeTeam].GoalsScore - standings[match.HomeTeam].GoalsConceded;
                        standings[match.AwayTeam].GoalsDifference = standings[match.AwayTeam].GoalsScore - standings[match.AwayTeam].GoalsConceded;
                    }
                    else
                    {
                        standings[match.AwayTeam].Points += 3;
                        standings[match.AwayTeam].GoalsScore += match.AwayScore;
                        standings[match.HomeTeam].GoalsConceded += match.AwayScore;
                        standings[match.AwayTeam].GoalsConceded += match.HomeScore;
                        standings[match.HomeTeam].GoalsScore += match.HomeScore;
                        standings[match.HomeTeam].GamePlayed++;
                        standings[match.AwayTeam].GamePlayed++;
                        standings[match.HomeTeam].TeamName = match.HomeTeam;
                        standings[match.AwayTeam].TeamName = match.AwayTeam;
                        standings[match.HomeTeam].GoalsDifference = standings[match.HomeTeam].GoalsScore - standings[match.HomeTeam].GoalsConceded;
                        standings[match.AwayTeam].GoalsDifference = standings[match.AwayTeam].GoalsScore - standings[match.AwayTeam].GoalsConceded;
                    }

                }
        }
        var list = standings.Values.OrderByDescending(s => s.Points).OrderByDescending(s => s.GoalsDifference).ToList();
        logger.LogInformation("the list of the groups {total}", list.Count);
        StandingsTable table = new()
        {
            ID = Guid.NewGuid().ToString(),
            Standings = list,
            TournamentID = tournamentID
        };
        // logger.LogInformation("table information {info}", table.Standings.Count);
        databaseContext.StandingsTables.Add(table);
        await databaseContext.SaveChangesAsync();
        return list;
    }
}