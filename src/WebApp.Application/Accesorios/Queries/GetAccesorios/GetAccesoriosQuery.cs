using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Accesorios.Queries.GetAccesorios;

public class GetAccesoriosQuery
{
    public record GetAccesoriosQueryRequest : IRequest<Result<List<GetAccesoriosResponse>>>;
}