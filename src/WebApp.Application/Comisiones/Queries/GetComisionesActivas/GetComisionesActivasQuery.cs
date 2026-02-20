using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesActivas;

public class GetComisionesActivasQuery
{
    public record GetComisionActivaQueryRequest : IRequest<Result<GetComisionActivaResponse>>
    {
        public int UsuarioId { get; set; }
    };
}