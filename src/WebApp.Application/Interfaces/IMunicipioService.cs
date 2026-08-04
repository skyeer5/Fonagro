using WebApp.Application.Municipios.Queries.GetMunicipiosByDep;

namespace WebApp.Application.Interfaces;

public interface IMunicipioService
{
    Task<List<GetMunicipiosByDepResponse>> GetMunicipiosByDepAsync(int departamentoId);
    bool MunicipiosExists(List<int> municipiosIds);
}