namespace smarttournamentengine.STE.Entity;

public class Match
{
    public required string MatchID { get; set; }
    public required string HomeTeam { get; set; }
    public required string AwayTeam { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
}