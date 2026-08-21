using WebApp.Application.Municipios.Queries.GetMunicipiosByDep;

namespace WebApp.Application.Interfaces;

public interface IMunicipioService
{
    Task<List<GetMunicipiosByDepResponse>> GetMunicipiosByDepAsync(int departamentoId);
    Task<bool> MunicipiosExistsAsync(List<int> municipiosIds, CancellationToken cancellationToken);
}