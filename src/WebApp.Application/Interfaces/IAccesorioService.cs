using WebApp.Application.Accesorios.Queries.GetAccesorios;

namespace WebApp.Application.Interfaces
{
    public interface IAccesoriosService
    {
        Task<List<GetAccesoriosResponse>> GetAccesoriosListAsync(CancellationToken cancellationToken);
    }
}