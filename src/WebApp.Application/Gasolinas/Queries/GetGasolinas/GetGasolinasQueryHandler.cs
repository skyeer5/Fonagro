using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Gasolinas.Queries.GetGasolinas;

public class GetGasolinasQueryHandler : IRequestHandler<GetGasolinasQuery.GetGasolinasQueryRequest, Result<List<GetGasolinasResponse>>>
{
    private readonly IGasolinaService _gasolinaService;

    public GetGasolinasQueryHandler(IGasolinaService gasolinaService)
    {
        _gasolinaService = gasolinaService;
    }

    public async Task<Result<List<GetGasolinasResponse>>> Handle(GetGasolinasQuery.GetGasolinasQueryRequest request, CancellationToken cancellationToken)
    {
        var gasolinas = await _gasolinaService.GetGasolinasListAsync(cancellationToken);
        if (gasolinas is null)
        {
            return Result<List<GetGasolinasResponse>>.Failure("No se encontraron gasolinas.");
        }
        return Result<List<GetGasolinasResponse>>.Success(gasolinas);
    }
}