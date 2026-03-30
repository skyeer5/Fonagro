using WebApp.Application.Comisiones.Command.ComisionAddDestinos;
using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IComisionRepository
{
    Task<Result<int>> AddAsync(Domain.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> UpdateComisionAsync(Domain.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> AddDestinosAsync(Domain.Comision comision, List<ComisionAddDestinosItemRequest> destinos, CancellationToken cancellationToken);
    Task<Result<int>> AddApprovalGasAsync(Domain.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> CheckComisionStatusAsync(CancellationToken cancellationToken);
}