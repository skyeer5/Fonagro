using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;

namespace WebApp.Application.Viaticos.Queries.GetViaticosVigentes;

public class GetViaticosVigentesQueryHandler : IRequestHandler<GetViaticosVigentesQuery.GetViaticosVigentesQueryRequest, Result<List<GetViaticosVigentesResponse>>>
{
    private readonly IViaticosService _viaticosService;

    public GetViaticosVigentesQueryHandler(IViaticosService viaticosService)
    {
        _viaticosService = viaticosService;
    }

    public async Task<Result<List<GetViaticosVigentesResponse>>> Handle(GetViaticosVigentesQuery.GetViaticosVigentesQueryRequest request, CancellationToken cancellationToken)
    {
        var viaticos = await _viaticosService.GetViaticosVigentesAsync();
        if(viaticos.Count == 0 || viaticos is null)
        {
            return Result<List<GetViaticosVigentesResponse>>.Failure("No se encontraron viáticos vigentes.");
        }
        return Result<List<GetViaticosVigentesResponse>>.Success(viaticos);
    }
}