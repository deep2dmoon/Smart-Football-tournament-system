namespace smarttournamentengine.STE.Entity;

public class Fixtures
{
    public required string TournamentID { get; set; }
    public required string ID { get; set; }
    public List<Match> Matches { get; set; } = [];
}