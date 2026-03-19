using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;

public class GetGasolinasWithPrecioQueryHandler : IRequestHandler<GetGasolinasWithPrecioQuery.GetGasolinasWithPrecioQueryRequest, Result<List<GetGasolinasWithPrecioResponse>>>
{
    private readonly IGasolinaService _gasolinaService;

    public GetGasolinasWithPrecioQueryHandler(IGasolinaService gasolinaService)
    {
        _gasolinaService = gasolinaService;
    }

    public async Task<Result<List<GetGasolinasWithPrecioResponse>>> Handle(GetGasolinasWithPrecioQuery.GetGasolinasWithPrecioQueryRequest request, CancellationToken cancellationToken)
    {
        var gasolinas = await _gasolinaService.GetGasolinasWithPrecioListAsync(cancellationToken);
        if (gasolinas is null)
        {
            return Result<List<GetGasolinasWithPrecioResponse>>.Failure("No se encontraron gasolinas.");
        }
        return Result<List<GetGasolinasWithPrecioResponse>>.Success(gasolinas);
    }
}