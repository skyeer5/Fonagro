using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;

public class GetUsuariosSinComisionQuery
{
    public record GetUsuariosSinComisionQueryRequest : IRequest<Result<List<GetUsuariosSinComisionResponse>>>;
}