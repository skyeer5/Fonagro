using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IComisionRepository
{
    Task<Result<int>> AddAsync(Domain.Comision comision, CancellationToken cancellationToken);
    Task<Result<int>> UpdateComisionAsync(Domain.Comision comision, CancellationToken cancellationToken);
}