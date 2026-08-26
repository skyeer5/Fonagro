using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Comisiones.Queries.GetComisionesPendApprov.GetComisionesPendApprovQuery;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovQueryHandler: IRequestHandler<GetComisionesPendApprovQueryRequest, Result<PagedList<GetComisionesPendApprovResponse>>>
{
    private readonly IComisionService _comisionService;

    public GetComisionesPendApprovQueryHandler(IComisionService comisionService)
    {
        _comisionService = comisionService;
    }

    public async Task<Result<PagedList<GetComisionesPendApprovResponse>>> Handle(GetComisionesPendApprovQueryRequest request, CancellationToken cancellationToken)
    {
        var comisiones = await _comisionService.GetComisionPendApprovAsync(request.Request);
        if (comisiones is null)
        {
             return Result<PagedList<GetComisionesPendApprovResponse>>.Failure("No se encontró comisión pendientes por aprobar gasolina.");
        }

        return Result<PagedList<GetComisionesPendApprovResponse>>.Success(comisiones);
    }
}