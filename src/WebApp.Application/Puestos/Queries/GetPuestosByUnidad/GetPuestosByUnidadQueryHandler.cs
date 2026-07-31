using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Puestos.Queries.GetPuestosByUnidad.GetPuestosByUnidadQuery;

namespace WebApp.Application.Puestos.Queries.GetPuestosByUnidad;

public class GetPuestosByUnidadQueryHandler : IRequestHandler<GetPuestosByUnidadQueryRequest, Result<List<GetPuestosByUnidadResponse>>>
{
    private readonly IPuestoService _puestoService;
    public GetPuestosByUnidadQueryHandler(IPuestoService puestoService)
    {
        _puestoService = puestoService;
    }
    public async Task<Result<List<GetPuestosByUnidadResponse>>> Handle(GetPuestosByUnidadQueryRequest request, CancellationToken cancellationToken)
    {
        var puestos = await _puestoService.GetPuestosByUnidadAsync(request.unidadId);
        if(puestos is null)
        {
            return Result<List<GetPuestosByUnidadResponse>>.Failure("Error al obtener los puestos");
        }
        return Result<List<GetPuestosByUnidadResponse>>.Success(puestos);
    }
}