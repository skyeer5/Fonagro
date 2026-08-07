using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Queries.GetNombramientos.GetNomParaAprobarQuery;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientos;

public class GetNombramientosQueryHandler : IRequestHandler<GetNombramientosQueryRequest, Result<PagedList<GetNombramientosResponse>>>
{
    private readonly INombramientoService _nombramientoService;
    public GetNombramientosQueryHandler(INombramientoService nombramientoService)
    {
        _nombramientoService = nombramientoService;
    }

    public async  Task<Result<PagedList<GetNombramientosResponse>>> Handle(GetNombramientosQueryRequest request, CancellationToken cancellationToken)
    {
        var nombramientos = await _nombramientoService.GetNombramientosAsync(request.request, cancellationToken);

        if(nombramientos is null)
        {
            return Result<PagedList<GetNombramientosResponse>>.Failure("Error al obtener los nombramientos");
        }

        return Result<PagedList<GetNombramientosResponse>>.Success(nombramientos);
    }
}