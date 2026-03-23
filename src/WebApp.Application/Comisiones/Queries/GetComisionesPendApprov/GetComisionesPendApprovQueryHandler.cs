using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Comisiones.Queries.GetComisionesPendApprov.GetComisionesPendApprovQuery;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovQueryHandler
            : IRequestHandler<GetComisionesPendApprovQueryRequest, Result<List<GetComisionesPendApprovResponse>>>
{
    private readonly IComisionService _comisionService;

    public GetComisionesPendApprovQueryHandler(IComisionService comisionService)
    {
        _comisionService = comisionService;
    }

    public async Task<Result<List<GetComisionesPendApprovResponse>>> Handle(GetComisionesPendApprovQueryRequest request, CancellationToken cancellationToken)
    {
        var comisiones = await _comisionService.GetComisionPendApprovAsync();
        if (comisiones is null)
        {
             return Result<List<GetComisionesPendApprovResponse>>.Failure("No se encontró comisión pendientes por aprobar gasolina.");
        }

        return Result<List<GetComisionesPendApprovResponse>>.Success(comisiones);
    }
}