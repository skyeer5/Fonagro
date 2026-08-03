using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Municipios.Queries.GetMunicipiosByDep;

public class GetMunicipiosByDepQueryHandler : IRequestHandler<GetMunicipiosByDepQuery.GetMunicipiosByDepQueryRequest, Result<List<GetMunicipiosByDepResponse>>>
{
    private readonly IMunicipioService _municipioService;

    public GetMunicipiosByDepQueryHandler(IMunicipioService municipioService)
    {
        _municipioService = municipioService;
    }

    public async Task<Result<List<GetMunicipiosByDepResponse>>> Handle(GetMunicipiosByDepQuery.GetMunicipiosByDepQueryRequest request, CancellationToken cancellationToken)
    {
        var municipios = await _municipioService.GetMunicipiosByDepAsync(request.departamentoId);
        if(municipios is null)
        {
            return Result<List<GetMunicipiosByDepResponse>>.Failure("Error al obtener los municipios");
        }
        return Result<List<GetMunicipiosByDepResponse>>.Success(municipios);
    }
}