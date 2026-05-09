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