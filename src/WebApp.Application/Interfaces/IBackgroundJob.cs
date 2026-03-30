namespace WebApp.Application.Interfaces;

public interface IBackgroundJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}