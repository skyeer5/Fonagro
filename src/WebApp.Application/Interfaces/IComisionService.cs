using WebApp.Application.Comisiones.Queries.GetComisionesActivas;

namespace WebApp.Application.Interfaces;

public interface IComisionService
{
    Task<List<GetComisionesActivasResponse>> GetComisionesActivasListAsync(int usuarioId);
}