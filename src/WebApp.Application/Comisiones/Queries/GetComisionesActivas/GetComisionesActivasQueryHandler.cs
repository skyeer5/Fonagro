using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Comisiones.Queries.GetComisionesActivas;

public class GetComisionesActivasQueryHandler
            : IRequestHandler<GetComisionesActivasQuery.GetComisionesActivasQueryRequest, Result<List<GetComisionesActivasResponse>>>
{
    private readonly IComisionService _comisionService;

    public GetComisionesActivasQueryHandler(IComisionService comisionService)
    {
        _comisionService = comisionService;
    }

    public async Task<Result<List<GetComisionesActivasResponse>>> Handle(GetComisionesActivasQuery.GetComisionesActivasQueryRequest request, CancellationToken cancellationToken)
    {
        var comisiones = await _comisionService.GetComisionesActivasListAsync(request.UsuarioId);
        return Result<List<GetComisionesActivasResponse>>.Success(comisiones);
    }
}