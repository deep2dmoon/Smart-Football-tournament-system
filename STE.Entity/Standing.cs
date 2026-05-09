namespace smarttournamentengine.STE.Entity;

public class Standing
{
    public required string ID{ get; set;}
    public required string TeamName { get; set; }
    public int GamePlayed { get; set; }
    public int GoalsDifference { get; set; }
    public int GoalsScore { get; set; }
    public int GoalsConceded { get; set; }
    public int Points { get; set; }
}