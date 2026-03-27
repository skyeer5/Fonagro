using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Comisiones.Queries.GetComisionesDetalle.GetComisionesDetalleQuery;

namespace WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

public class GetComisionesDetalleQueryHandler : IRequestHandler<GetComisionesDetalleQueryRequest, Result<PagedList<GetComisionesDetalleResponse>>>
{
    private readonly IComisionService _comision;

    public GetComisionesDetalleQueryHandler(IComisionService comision)
    {
        _comision = comision;
    }

    public async Task<Result<PagedList<GetComisionesDetalleResponse>>> Handle(GetComisionesDetalleQueryRequest request, CancellationToken cancellationToken)
    {
        return await _comision.GetComisionesDetalleAsync(request.request);

    }
}