namespace smarttournamentengine.STE.Entity;

public class User
{
    public required string ID { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Entity.Role? Role { get; set; } = Entity.Role.Guest;
}


public enum Role
{
    Guest, Admin, Player
}