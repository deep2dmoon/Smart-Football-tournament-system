using System.Text.Json.Serialization;
using smarttournamentengine.STE.Entity;

namespace smarttournamentengine;

public class Tournament
{

    public required string TournamentID { get; set; }
    public required string Name { get; set; }
    public List<Team> Participants { get; set; } = [];
    public List<Group> Groups { get; set; } = [];
    public List<Fixtures> Fixtures { get; set; } = [];
    public TournamentMode TournamentMode { get; set; } = TournamentMode.Pending;


}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TournamentMode
{
    Pending,
    Started,
    Suspended,
    Completed
}