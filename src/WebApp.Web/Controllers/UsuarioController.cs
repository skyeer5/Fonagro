using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Core;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Domain;
using static WebApp.Application.Usuarios.Commands.UsuarioCreate.UsuarioCreateCommand;

namespace WebApp.Web.Controllers;

public class UsuarioController : Controller
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public IActionResult Crear()
    {
        ViewBag.Puestos = UsuariosTipos.GetPuestos();
        ViewBag.Encargados = UsuariosTipos.GetEncargados();
        ViewBag.Unidades = UsuariosTipos.GetUnidades();
        ViewBag.Auxiliares = UsuariosTipos.GetAuxiliares();
        ViewBag.TipoServicios = UsuariosTipos.GetTipoServicios();
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