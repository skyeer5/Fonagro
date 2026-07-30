using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Unidades.Queries.GetUnidades.GetUnidadesQuery;

namespace WebApp.Application.Unidades.Queries.GetUnidades;

public class GetUnidadesQueryHandler : IRequestHandler<GetUnidadesQueryRequest, Result<List<GetUnidadesResponse>>>
{
    private readonly IUnidadService _unidadService;

    public GetUnidadesQueryHandler(IUnidadService unidadService)
    {
        _unidadService = unidadService;
    }
    public async Task<Result<List<GetUnidadesResponse>>> Handle(GetUnidadesQueryRequest request, CancellationToken cancellationToken)
    {
        var unidades = await _unidadService.GetUnidadesAsync();
        if(unidades is null)
        {
            return Result<List<GetUnidadesResponse>>.Failure("Error al obtener las unidades.");
        }
        return Result<List<GetUnidadesResponse>>.Success(unidades);
    }
}