using System.Text.Json.Serialization;

namespace smarttournamentengine.STE.Entity;

public class User
{
    public required string ID { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string Role { get; set; } = "";
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Guest, Admin, Player, Executive
}