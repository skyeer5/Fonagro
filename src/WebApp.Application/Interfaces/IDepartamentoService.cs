using WebApp.Application.Departamentos.Queries.GetDepartamentos;

namespace WebApp.Application.Interfaces;

public interface IDepartamentoService
{
    Task<List<GetDepartamentosResponse>> GetDepartamentosAsync();
}