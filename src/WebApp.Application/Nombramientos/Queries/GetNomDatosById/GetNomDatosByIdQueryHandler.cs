using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Queries.GetNomDatosById.GetNomDatosByIdQuery;

namespace WebApp.Application.Nombramientos.Queries.GetNomDatosById;

public class GetNomDatosByIdQueryHandler : IRequestHandler<GetNomDatosByIdQueryRequest, Result<GetNomDatosByIdResponse>>
{
    private readonly INombramientoService _nombramientoService;

    public GetNomDatosByIdQueryHandler(INombramientoService nombramientoService)
    {
        _nombramientoService = nombramientoService;
    }

    public async Task<Result<GetNomDatosByIdResponse>> Handle(GetNomDatosByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var nombramiento = await _nombramientoService.GetNomDatosByIdAsync(request.nombramientoId, cancellationToken);
        if(nombramiento is null)
        {
            return Result<GetNomDatosByIdResponse>.Failure("Error al obtener el nombramiento");
        }
        return Result<GetNomDatosByIdResponse>.Success(nombramiento);
    }
}