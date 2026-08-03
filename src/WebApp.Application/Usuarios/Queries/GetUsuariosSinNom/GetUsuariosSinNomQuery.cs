using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosSinNom;

public class GetUsuariosSinNomQuery
{
    public record GetUsuariosSinNomQueryRequest : IRequest<Result<List<GetUsuariosSinNomResponse>>>;
}