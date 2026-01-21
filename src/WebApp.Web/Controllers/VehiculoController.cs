using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Core;
using WebApp.Application.Vehiculos.Commands.VehiculoCreate;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
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

    public IActionResult Crear()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult<Result<int>>> Crear(
        [FromForm] VehiculoCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new VehiculoCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? View(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("ObtenerVehiculo/{id}")]
    public async Task<ActionResult<Result<GetVehiculoResponse>>> ObtenerVehiculo(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetVehiculoQueryRequest{Id = id};
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? View(result.Value) : NotFound(result.Error);
    }
}