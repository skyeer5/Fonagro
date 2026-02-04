using WebApp.Application.Partes.Queries.GetPartes;

namespace WebApp.Application.Interfaces
{
    public interface IPartesService
    {
        Task<List<GetPartesResponse>> GetPartesListAsync(CancellationToken cancellationToken);
    }
}   