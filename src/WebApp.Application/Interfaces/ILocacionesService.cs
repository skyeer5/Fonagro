using WebApp.Application.Locaciones.Queries;
using WebApp.Domain.Departamentos;

namespace WebApp.Application.Interfaces;

public interface ILocacionesService
{
    Task<List<Departamentos>?> GetDepartamentosAsync();
    Task<List<Departamento>?> GetDepartamentosDataSeedAsync();
}