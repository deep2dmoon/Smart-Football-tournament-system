


namespace smarttournamentengine.STE.Application.Workers;

public class Streamer(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            TournamentEngine tournamentEngine = scope.ServiceProvider.GetRequiredService<TournamentEngine>();
            await tournamentEngine.PlayMatch();


            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
        throw new NotImplementedException();
    }
}