using WebApp.Application.Interfaces;

namespace WebApp.Infrastructure.Jobs;

public class NombramientoEstadoJob : IBackgroundJob
{
    private readonly INombramientoRepository _nombramientoRepository;

    public NombramientoEstadoJob(INombramientoRepository nombramientoRepository)
    {
        _nombramientoRepository = nombramientoRepository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // await _nombramientoRepository.CompletarNombramientoStatusAsync(cancellationToken);
        await _nombramientoRepository.CancelarNombramientoStatusAsync(cancellationToken);
    }
}