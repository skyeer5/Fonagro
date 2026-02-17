using WebApp.Application.Viaticos.Queries.GetViaticosVigentes;

namespace WebApp.Application.Interfaces;

public interface IViaticosService
{
    Task<List<GetViaticosVigentesResponse>> GetViaticosVigentesAsync();
}