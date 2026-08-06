using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Queries.GetNomParaAprobar.GetNomParaAprobarQuery;

namespace WebApp.Application.Nombramientos.Queries.GetNomParaAprobar;

public class GetNomParaAprobarQueryHandler : IRequestHandler<GetNomParaAprobarQueryRequest, Result<PagedList<GetNomParaAprobarResponse>>>
{
    private readonly INombramientoService _nombramientoService;
    public GetNomParaAprobarQueryHandler(INombramientoService nombramientoService)
    {
        _nombramientoService = nombramientoService;
    }

    public async  Task<Result<PagedList<GetNomParaAprobarResponse>>> Handle(GetNomParaAprobarQueryRequest request, CancellationToken cancellationToken)
    {
        var nombramientos = await _nombramientoService.GetListNMParaAprobarAsync(request.request, cancellationToken);

        if(nombramientos is null)
        {
            return Result<PagedList<GetNomParaAprobarResponse>>.Failure("Error al obtener los nombramientos");
        }

        return Result<PagedList<GetNomParaAprobarResponse>>.Success(nombramientos);
    }
}