using System.Text.Json.Serialization;

namespace smarttournamentengine.STE.Entity;

public class Team
{
    public required string TeamID { get; set; }
    public required string Name { get; set; }
    public TournamentMode TournamentMode { get; set; } = TournamentMode.Running;

}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TournamentMode
{
    Running,
    Suspended,
    DisQualified
}