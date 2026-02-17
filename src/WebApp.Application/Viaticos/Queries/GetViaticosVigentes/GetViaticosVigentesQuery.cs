using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Viaticos.Queries.GetViaticosVigentes;

public class GetViaticosVigentesQuery 
{
    public record GetViaticosVigentesQueryRequest : IRequest<Result<List<GetViaticosVigentesResponse>>>;
}