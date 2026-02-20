using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.ComisionCreate;
using WebApp.Application.Comisiones.Command.ComisionAddDestinos;
using WebApp.Application.Core;
using WebApp.Web.Models;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;
using static WebApp.Application.Comisiones.Command.ComisionAddDestinos.ComisionAddDestinosCommand;
using static WebApp.Application.Comisiones.Queries.GetComisionesActivas.GetComisionesActivasQuery;
using static WebApp.Application.Usuarios.Queries.GetUsuariosSinComision.GetUsuariosSinComisionQuery;
using static WebApp.Application.Vehiculos.Queries.GetVehiculosDisponibles.GetVehiculosDisponiblesQuery;

namespace WebApp.Web.Controllers;

[Route("comision")]
public class ComisionController : Controller
{
    private readonly IMediator _mediator;
    public ComisionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{usuarioid}")]
    public async Task<ActionResult<ComisionViewModel>> Index(int usuarioid)
    {
        var resultado = await _mediator.Send(new GetComisionActivaQueryRequest{UsuarioId = usuarioid});

        var vm = new ComisionViewModel
        {
            Comision = resultado.Value,
            TieneComisionCreada = resultado.IsSuccess,
            TieneDestinosDefinidos = false,
            TieneCombustiblesAprobados = false,
            TieneComisionLista = false,
            TieneComisionEnCurso = false
        };

        if(!resultado.IsSuccess)
        {
            vm.TieneComisionCreada = false;
            return View(vm);
        }
        if(resultado.Value!.destinos!.Any())
        {
            vm.TieneDestinosDefinidos = true;
        }
        return View(vm);
    }
    [HttpGet("Crear")]
    public IActionResult Crear()
    {
        var usuarios = _mediator.Send(new GetUsuariosSinComisionQueryRequest());
        if(!usuarios.Result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, usuarios.Result.Error!);
            return View();
        }
        ViewBag.Usuarios = new SelectList(usuarios.Result.Value, "Id", "Nombre_Completo");

        var vehiculos = _mediator.Send(new GetVehiculosDisponiblesQueryRequest());
        if(!vehiculos.Result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, vehiculos.Result.Error!);
            return View();
        }
        ViewBag.Vehiculos = new SelectList(vehiculos.Result.Value, "id", "Descripcion");

        return View();
    }
    [HttpPost("Crear")]
    public async Task<ActionResult<Result<int>>> Crear(
        [FromForm] ComisionCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ComisionCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction("ObtenerComision", new { id = result.Value }) : BadRequest(result.Error);
    }
    [HttpPost("AgregarDestinos")]
    public async Task<ActionResult<Result<int>>> AgregarDestinos(
        [FromForm] ComisionAddDestinosRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ComisionAddDestinosCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction("ObtenerComision", new { id = result.Value }) : BadRequest(result.Error);
    }

    // [HttpGet("Detalle/{id}")]
    // public async Task<ActionResult<Result<GetVehiculoResponse>>> Detalle(
    //     int id,
    //     CancellationToken cancellationToken
    // )
    // {
    //     var query = new GetVehiculoQueryRequest{Id = id};
    //     var result = await _mediator.Send(query, cancellationToken);
    //     return result.IsSuccess ? View(result.Value) : NotFound(result.Error);
    // }
}