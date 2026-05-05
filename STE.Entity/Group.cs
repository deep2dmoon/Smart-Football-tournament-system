namespace smarttournamentengine.STE.Entity;

public class Group
{
    public required string GroupID { get; set; }
    public List<Team> Teams { get; set; } = [];
}