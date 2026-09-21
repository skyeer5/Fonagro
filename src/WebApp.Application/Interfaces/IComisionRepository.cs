using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IComisionRepository
{
    void Add(Domain.Comisiones.Comision comision);
    void RemoveRangeDestinos(List<Domain.ComisionDestinos.ComisionDestino> destinos);
    Task<Result<int>> CheckComisionStatusAsync(CancellationToken cancellationToken);
    void AgregarCreadoPor(Domain.Comisiones.Comision comision);
    void AgregarAprobadoPor(Domain.Comisiones.Comision comision);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}