using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovQuery
{
    public record GetComisionesPendApprovQueryRequest : IRequest<Result<List<GetComisionesPendApprovResponse>>>;
}