using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Gasolinas.Queries.GetGasolinasWithFecha;

public class GetGasolinasWithPrecioQueryHandler : IRequestHandler<GetGasolinasWithFechaQuery.GetGasolinasWithFechaQueryRequest, Result<List<GetGasolinasWithFechaResponse>>>
{
    private readonly IGasolinaService _gasolinaService;

    public GetGasolinasWithPrecioQueryHandler(IGasolinaService gasolinaService)
    {
        _gasolinaService = gasolinaService;
    }

    public async Task<Result<List<GetGasolinasWithFechaResponse>>> Handle(GetGasolinasWithFechaQuery.GetGasolinasWithFechaQueryRequest request, CancellationToken cancellationToken)
    {
        var gasolinas = await _gasolinaService.GetGasolinasWithFechaListAsync(cancellationToken);
        if (gasolinas is null)
        {
            return Result<List<GetGasolinasWithFechaResponse>>.Failure("No se encontraron gasolinas.");
        }
        return Result<List<GetGasolinasWithFechaResponse>>.Success(gasolinas);
    }
}