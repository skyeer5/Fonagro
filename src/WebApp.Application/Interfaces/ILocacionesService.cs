using WebApp.Domain.Departamentos;

namespace WebApp.Application.Interfaces;

public interface ILocacionesService
{
    Task<List<Departamento>?> GetDepartamentosDataSeedAsync();
}