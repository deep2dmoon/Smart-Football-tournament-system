namespace smarttournamentengine.STE.Entity;

public class StandingsTable
{
    public required string ID { get; set; }
    public required string TournamentID { get; set; }
    public List<Standing> Standings { get; set; } = [];
}