using WebApp.Application.Interfaces;

namespace WebApp.Infrastructure.Jobs;

public class ComisionEstadoJob : IBackgroundJob
{
    private readonly IComisionRepository _comisionRepository;

    public ComisionEstadoJob(IComisionRepository comisionRepository)
    {
        _comisionRepository = comisionRepository;
    }
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return _comisionRepository.CheckComisionStatusAsync(cancellationToken);
    }
}