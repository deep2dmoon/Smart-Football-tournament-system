namespace smarttournamentengine.STE.Entity;

public class Fixtures
{
    public required string FixtureId { get; set; }
    public List<Match>? Matches { get; set; }
}