using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Partes.Queries.GetPartes;

public class GetPartesQueryHandler : IRequestHandler<GetPartesQuery.GetPartesQueryRequest, WebApp.Application.Core.Result<List<GetPartesResponse>>>
{
    private readonly IPartesService _partesService;
    public GetPartesQueryHandler(IPartesService partesService)
    {
        _partesService = partesService;
    }
    public async Task<WebApp.Application.Core.Result<List<GetPartesResponse>>> Handle(GetPartesQuery.GetPartesQueryRequest request, CancellationToken cancellationToken)
    {

        var partes = await _partesService.GetPartesListAsync(cancellationToken);
        if (partes.IsNullOrEmpty())
        {
            return WebApp.Application.Core.Result<List<GetPartesResponse>>.Failure("No se encontraron partes.");
        }
        return WebApp.Application.Core.Result<List<GetPartesResponse>>.Success(partes);
    }
}