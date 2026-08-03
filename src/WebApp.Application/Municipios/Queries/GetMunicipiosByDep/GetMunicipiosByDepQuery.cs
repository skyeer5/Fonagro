using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Municipios.Queries.GetMunicipiosByDep;

public class GetMunicipiosByDepQuery
{
    public record GetMunicipiosByDepQueryRequest(int departamentoId) : IRequest<Result<List<GetMunicipiosByDepResponse>>>;
}