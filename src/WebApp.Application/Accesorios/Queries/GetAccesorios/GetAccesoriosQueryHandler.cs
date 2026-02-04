using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Accesorios.Queries.GetAccesorios;

public class GetAccesoriosQueryHandler : IRequestHandler<GetAccesoriosQuery.GetAccesoriosQueryRequest, Result<List<GetAccesoriosResponse>>>
{
    private readonly IAccesoriosService _accesoriosService;

    public GetAccesoriosQueryHandler(IAccesoriosService accesoriosService)
    {
        _accesoriosService = accesoriosService;
    }

    public async Task<Result<List<GetAccesoriosResponse>>> Handle(GetAccesoriosQuery.GetAccesoriosQueryRequest request, CancellationToken cancellationToken)
    {
        var accesorios = await _accesoriosService.GetAccesoriosListAsync(cancellationToken);
        if (accesorios.IsNullOrEmpty())
        {
            return Result<List<GetAccesoriosResponse>>.Failure("No se encontraron accesorios.");
        }           

        return Result<List<GetAccesoriosResponse>>.Success(accesorios);
    }
}