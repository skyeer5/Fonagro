using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovQuery
{
    public record GetComisionesPendApprovQueryRequest(GetComisionesPendApprovRequest Request) : IRequest<Result<PagedList<GetComisionesPendApprovResponse>>>;
}