using WebApp.Application.Puestos.Queries.GetPuestosByUnidad;
using WebApp.Domain.Puestos;

namespace WebApp.Application.Interfaces;

public interface IPuestoService
{
    Task<List<GetPuestosByUnidadResponse>> GetPuestosByUnidadAsync(int unidadId);
    Task<bool> ExistsAsync(int puestoId);
}