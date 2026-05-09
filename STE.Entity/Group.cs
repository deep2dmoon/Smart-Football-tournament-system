namespace smarttournamentengine.STE.Entity;

public class Group
{
    public required string GroupID { get; set; }
    public required string TournamentID { get; set; }
    public List<Team> Teams { get; set; } = [];
    public List<Match> Matches { get; set; } = [];
}