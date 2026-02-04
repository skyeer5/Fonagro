using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivos;

public class GetUsuariosActivosQuery
{
    public record GetUsuariosActivosQueryRequest : IRequest<Result<List<GetUsuariosActivosResponse>>>;
}