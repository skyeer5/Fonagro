using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;

public class GetUsuariosActivosDetalleQuery
{
    public record GetUsuariosActivosDetalleQueryRequest(GetUsuariosActivosDetalleRequest request) : IRequest<Result<PagedList<GetUsuariosActivosDetalleResponse>>>;
}