using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNomParaAprobar;

public class GetNomParaAprobarQuery
{
    public record GetNomParaAprobarQueryRequest(GetNomParaAprobarRequest request) : IRequest<Result<PagedList<GetNomParaAprobarResponse>>>;
}