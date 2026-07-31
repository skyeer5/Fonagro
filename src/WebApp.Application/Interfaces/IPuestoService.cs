using WebApp.Application.Puestos.Queries.GetPuestosByUnidad;

namespace WebApp.Application.Interfaces;

public interface IPuestoService
{
    Task<List<GetPuestosByUnidadResponse>> GetPuestosByUnidadAsync(int unidadId);
}