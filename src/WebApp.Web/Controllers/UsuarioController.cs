using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Core;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using static WebApp.Application.Usuarios.Commands.UsuarioCreate.UsuarioCreateCommand;

namespace WebApp.Web.Controllers;

[Route("usuario")]
public class UsuarioController : Controller
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public IActionResult Crear()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult<Result<int>>> Crear(
        [FromForm] UsuarioCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UsuarioCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction("Home/Index") : BadRequest(result.Error);
    }
}