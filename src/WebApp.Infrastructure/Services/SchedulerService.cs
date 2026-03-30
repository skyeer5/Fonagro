using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Application.Interfaces;

namespace WebApp.Infrastructure.Services;

public class SchedulerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SchedulerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var jobs = scope.ServiceProvider.GetServices<IBackgroundJob>();

            foreach (var job in jobs)
            {
                await job.ExecuteAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}