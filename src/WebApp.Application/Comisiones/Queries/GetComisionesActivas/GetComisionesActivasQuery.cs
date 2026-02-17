using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesActivas;

public class GetComisionesActivasQuery
{
    public record GetComisionesActivasQueryRequest : IRequest<Result<List<GetComisionesActivasResponse>>>
    {
        public int UsuarioId { get; set; }
    };
}