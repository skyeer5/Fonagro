using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Locaciones.Queries.GetDepartamentos
{
    public class GetDepartamentosQuery
    {
        public record GetDepartamentosQueryRequest : IRequest<Result<GetDepartamentosResponse>>;
    }
}