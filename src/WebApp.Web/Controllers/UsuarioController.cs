using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Core;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Domain;
using static WebApp.Application.Usuarios.Commands.UsuarioCreate.UsuarioCreateCommand;
using static WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle.GetUsuariosActivosDetalleQuery;

namespace WebApp.Web.Controllers;
[Authorize]
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
        if(!result.IsSuccess)
        {
            ViewBag.Puestos = UsuariosTipos.GetPuestos();
            ViewBag.Encargados = UsuariosTipos.GetEncargados();
            ViewBag.Unidades = UsuariosTipos.GetUnidades();
            ViewBag.Auxiliares = UsuariosTipos.GetAuxiliares();
            ViewBag.TipoServicios = UsuariosTipos.GetTipoServicios();
            TempData["msg"] = result.Error;
            return View(request);
        }
        return RedirectToAction(nameof(List));
    }
    [HttpGet]
    public async Task<IActionResult> List(string? nombre = "", string? puesto = "", string? unidad = "", string? estado = "", int currentPage = 1, string orderBy = "")
    {
        ViewBag.Estados = UsuarioEstados.GetEstados();
        var request = new GetUsuariosActivosDetalleRequest
        {
            Nombre = nombre,
            Puesto = puesto,
            Unidad = unidad,
            Estado = estado,
            PageNumber = currentPage,
            OrderBy = orderBy
        };
        var query = new GetUsuariosActivosDetalleQueryRequest(request);
        var result = await _mediator.Send(query);
        return View(result.Value);
    }
}