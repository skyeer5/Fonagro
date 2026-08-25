using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.NombramientoPdf;

public class NombramientoPdfQuery
{
    public record NombramientoPdfQueryRequest(int nombramientoId) : IRequest<Result<NombramientoPdfResponse>>;
}