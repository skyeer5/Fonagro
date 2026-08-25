using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Usuarios.Queries.GetUsuarios;

public class GetUsuariosQuery
{
    public record GetUsuariosQueryRequest() : IRequest<Result<List<GetUsuariosResponse>>>;
}