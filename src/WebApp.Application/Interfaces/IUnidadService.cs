using WebApp.Application.Unidades.Queries.GetUnidades;

namespace WebApp.Application.Interfaces;

public interface IUnidadService
{
    Task<List<GetUnidadesResponse>> GetUnidadesAsync();
}