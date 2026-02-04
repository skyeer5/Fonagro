using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Partes.Queries.GetPartes;

public class GetPartesQuery
{
    public record GetPartesQueryRequest : IRequest<Result<List<GetPartesResponse>>>;
}