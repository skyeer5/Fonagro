using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Queries.GetNombramientoById.GetNombramientoByIdQuery;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientoById;

public class GetNombramientoByIdQueryHandler : IRequestHandler<GetNombramientoByIdQueryRequest, Result<GetNombramientoByIdResponse>>
{
    private readonly INombramientoService _nombramientoService;

    public GetNombramientoByIdQueryHandler(INombramientoService nombramientoService)
    {
        _nombramientoService = nombramientoService;
    }

    public async Task<Result<GetNombramientoByIdResponse>> Handle(GetNombramientoByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var nombramiento = await _nombramientoService.GetNombramientoByIdResponseAsync(request.NombramientoId, cancellationToken);
        if(nombramiento is null)
            return Result<GetNombramientoByIdResponse>.Failure("No se encontró el nombramiento");
        
        return Result<GetNombramientoByIdResponse>.Success(nombramiento);
    }
}