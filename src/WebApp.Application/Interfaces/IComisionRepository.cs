using WebApp.Application.Comisiones.Command.ComisionAddDestinos;
using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IComisionRepository
{
    Task<Result<int>> AddAsync(Domain.Comisiones.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> UpdateComisionAsync(Domain.Comisiones.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> AddDestinosAsync(Domain.Comisiones.Comision comision, List<ComisionAddDestinosItemRequest> destinos, CancellationToken cancellationToken);
    Task<Result<int>> AddApprovalGasAsync(Domain.Comisiones.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> CheckComisionStatusAsync(CancellationToken cancellationToken);
    Task<Result<int>> CancelComisionAsync(int comisionId, CancellationToken cancellationToken);
}