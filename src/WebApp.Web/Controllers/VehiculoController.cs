using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Core;
using WebApp.Application.Partes.Queries.GetPartes;
using WebApp.Application.Vehiculos.Commands.VehiculoCreate;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
using static WebApp.Application.Accesorios.Queries.GetAccesorios.GetAccesoriosQuery;
using static WebApp.Application.Partes.Queries.GetPartes.GetPartesQuery;
using static WebApp.Application.Vehiculos.Commands.VehiculoCreate.VehiculoCreateCommand;
using static WebApp.Application.Vehiculos.Queries.GetVehiculo.GetVehiculoQuery;

namespace WebApp.Web.Controllers;

[Route("vehiculo")]
public class VehiculoController : Controller
{
    private readonly IMediator _mediator;
    public VehiculoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("Crear")]
    public IActionResult Crear()
    {
        var partes = _mediator.Send(new GetPartesQueryRequest());
        if(!partes.Result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, partes.Result.Error!);
            return View();
        }
        ViewBag.Partes = new SelectList(partes.Result.Value, "id", "Nombre");

        
        var accesorios = _mediator.Send(new GetAccesoriosQueryRequest());
        if (!accesorios.Result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, accesorios.Result.Error!);
            return View();
        }
        ViewBag.Accesorios = new SelectList(accesorios.Result.Value, "id", "Nombre");
        return View();
    }
    [HttpPost("Crear")]
    public async Task<ActionResult<Result<int>>> Crear(
        [FromForm] VehiculoCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new VehiculoCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction("ObtenerVehiculo", new { id = result.Value }) : BadRequest(result.Error);
    }

    [HttpGet("Detalle/{id}")]
    public async Task<ActionResult<Result<GetVehiculoResponse>>> Detalle(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetVehiculoQueryRequest{Id = id};
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? View(result.Value) : NotFound(result.Error);
    }
}