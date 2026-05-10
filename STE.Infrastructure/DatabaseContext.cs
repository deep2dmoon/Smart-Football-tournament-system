using Microsoft.EntityFrameworkCore;
using smarttournamentengine.STE.Entity;

namespace smarttournamentengine.STE.infrastructure;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamGroupID> TeamGroupIDs => Set<TeamGroupID>();
    public DbSet<Alphabet> Alphabets => Set<Alphabet>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<Fixtures> Fixtures => Set<Fixtures>();
    public DbSet<Standing> Standings => Set<Standing>();
    public DbSet<StandingsTable> StandingsTables => Set<StandingsTable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alphabet>().HasData(
        new Alphabet() { ID = 1, Value = "A" },
        new Alphabet() { ID = 2, Value = "B" },
        new Alphabet() { ID = 3, Value = "C" },
        new Alphabet() { ID = 4, Value = "E" },
        new Alphabet() { ID = 5, Value = "F" },
         new Alphabet() { ID = 6, Value = "G" },
          new Alphabet() { ID = 7, Value = "H" },
           new Alphabet() { ID = 8, Value = "I" },
            new Alphabet() { ID = 9, Value = "J" }
        );
        modelBuilder.Entity<User>().HasData(
            new User
            {
                ID = "executives-id",
                Email = "user@exec.football",
                Name = "John Doe",
                Password = "exec-pass",
                Role = "Executive"
            }
        );

        modelBuilder.Entity<Group>().HasMany(x => x.Matches).WithOne().OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<StandingsTable>().HasMany(x => x.Standings).WithOne().OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}