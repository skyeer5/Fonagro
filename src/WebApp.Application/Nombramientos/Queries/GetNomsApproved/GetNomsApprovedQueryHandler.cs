using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Queries.GetNomsApproved.GetNomsApprovedQuery;

namespace WebApp.Application.Nombramientos.Queries.GetNomsApproved;

public class GetNomsApprovedQueryHandler : IRequestHandler<GetNomsApprovedQueryRequest, Result<List<GetNomsApprovedResponse>>>
{
    private readonly INombramientoService _nombramientoService;

    public GetNomsApprovedQueryHandler(INombramientoService nombramientoService)
    {
        _nombramientoService = nombramientoService;
    }

    public async Task<Result<List<GetNomsApprovedResponse>>> Handle(GetNomsApprovedQueryRequest request, CancellationToken cancellationToken)
    {
        var nombramientos = await _nombramientoService.GetNomsApprovedAsync(cancellationToken);
        if(nombramientos is null)
        {
            return Result<List<GetNomsApprovedResponse>>.Failure("Error al obtener lo nombramientos");
        }
        return Result<List<GetNomsApprovedResponse>>.Success(nombramientos);
    }
}