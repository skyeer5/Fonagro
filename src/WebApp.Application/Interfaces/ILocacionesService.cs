using WebApp.Application.Locaciones.Queries;

namespace WebApp.Application.Interfaces;

public interface ILocacionesService
{
    Task<List<Departamentos>?> GetDepartamentosAsync();
}