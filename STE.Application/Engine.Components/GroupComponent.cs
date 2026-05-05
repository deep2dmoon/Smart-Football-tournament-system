using smarttournamentengine.STE.Entity;
using smarttournamentengine.STE.infrastructure;

namespace smarttournamentengine.STE.Application.Engine.Components;

public class GroupEngine(DatabaseContext context)
{
    private DatabaseContext databaseContext = context;
    public async Task<object> GroupTeams(string tournamentID, int groupNumber)
    {
        Tournament tournament = databaseContext.Tournaments.FirstOrDefault(t => t.TournamentID == tournamentID)!;
        List<Alphabet> alphabets = [.. databaseContext.Alphabets];

        Random random = new();
        int randomAlphabet = random.Next(0, alphabets.Count);

        if (groupNumber % 2 != 0)
            return new { response = "Group can only be evenly distributed" };

        if (tournament.Participants.Count < 2 || tournament.Participants.Count % 2 != 0)
            return new { response = "Add Tournament Participants,to evenly distribute groups" };
        List<Group> groups = [];
        List<Team> group = [];
        for (int i = 0; i < groupNumber; i++)
        {
            while (group.Count < 2)
            {
                int randomTeam = random.Next(0, tournament.Participants.Count);
                if (group.Contains(tournament.Participants[randomTeam]!)) continue;
                group.Add(tournament.Participants[randomTeam]);
                tournament.Participants.Remove(tournament.Participants[randomTeam]);
            }
            Group NewGroup = new() { GroupID = Guid.NewGuid().ToString("N"), Teams = group }; group = [];
            groups.Add(NewGroup);
        }
        tournament.Groups.AddRange(groups);
        await databaseContext.SaveChangesAsync();
        return new { response = "Participants Grouped", groups = tournament.Groups };
    }
}