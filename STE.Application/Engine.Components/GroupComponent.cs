using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application.Engine.Components;

public class GroupEngine(DatabaseContext context, ILogger<GroupEngine> logger_)
{
    private readonly ILogger<GroupEngine> logger = logger_;
    private DatabaseContext databaseContext = context;


    public async Task<Response>? GroupTeams(string tournamentID, int peerNumber)
    {
        Tournament tournament = databaseContext.Tournaments.Include(t =>
        t.Participants).FirstOrDefault(t => t.TournamentID == tournamentID)!;

        Random random = new();

        if (tournament != null)
        {
            if (tournament.Participants.Count < 2 || tournament.Participants.Count % 2 != 0)
                return new Response { Message = "Add Tournament Participants, to evenly distribute groups", code = 403, Status = Status.BadRequest };
        }
        else
        {
            return new Response { Message = "Tournament not found", Status = Status.NotFound, code = 404 };
        }
        if (databaseContext.Groups.FirstOrDefault(g => g.TournamentID == tournamentID) != null)
        {
            databaseContext.Groups.RemoveRange(databaseContext.Groups.Include(g => g.Matches).Where(g => g.TournamentID == tournamentID).ToList());
        }

        var bucket = tournament.Participants;
        var shuffled = bucket.OrderBy(t => random.Next()).ToList(); // shuffle the list
        var groups = new List<List<Team>>();

        for (int i = 0; i < shuffled.Count; i += peerNumber)
        {
            var group = shuffled.Skip(i).Take(peerNumber).ToList();
            groups.AddRange(group);
        }

        foreach (List<Team> group in groups)
        {
            Group NewGroup = new() { GroupID = Guid.NewGuid().ToString(), TournamentID = tournamentID };
            NewGroup.Teams.AddRange(group);
            databaseContext.Groups.Add(NewGroup);
        }

        databaseContext.SaveChanges();
        return new Response { Message = $"Participants Grouped into", Status = Status.Ok, code = 200 };
    }


    public List<Standing> GetGroupTableStandings(string groupID, string tournamentId)
    {

        Dictionary<string, Standing> standings = [];

        foreach (Group group in databaseContext.Groups.Include(g => g.Matches).Where(g => g.TournamentID == tournamentId))
        {
            if (group.GroupID == groupID)
            {
                foreach (Match match in group.Matches)
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
                        standings[match.HomeTeam].GoalsConceded += match.AwayScore;
                        standings[match.HomeTeam].GoalsScore += match.HomeScore;
                        standings[match.AwayTeam].GoalsConceded += match.HomeScore;
                        standings[match.AwayTeam].GoalsScore += match.AwayScore;
                        standings[match.HomeTeam].GamePlayed++;
                        standings[match.AwayTeam].GamePlayed++;
                        standings[match.AwayTeam].GoalsDifference = standings[match.AwayTeam].GoalsScore - standings[match.AwayTeam].GoalsConceded;
                        standings[match.HomeTeam].GoalsDifference = standings[match.HomeTeam].GoalsScore - standings[match.HomeTeam].GoalsConceded;
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
        }
        List<Standing> standingTable = [.. standings.Values.OrderByDescending(x => x.Points)];
        return standingTable;
    }
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    BadRequest,
    NotFound,
    Ok

}
public class Response
{
    public int code { get; set; }
    public Status Status { get; set; }
    public required string Message { get; set; }

}