using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNomsApproved;

public class GetNomsApprovedQuery
{
    public record GetNomsApprovedQueryRequest : IRequest<Result<List<GetNomsApprovedResponse>>>;
}