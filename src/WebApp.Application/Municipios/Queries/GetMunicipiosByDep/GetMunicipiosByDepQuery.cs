using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Municipios.Queries.GetMunicipiosByDep;

public class GetMunicipiosByDepQuery
{
    public record GetMunicipiosByDepQueryRequest(List<int> DepartamentosId) : IRequest<Result<List<GetMunicipiosByDepResponse>>>;
}