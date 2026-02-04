using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.ComisionCreate;
using WebApp.Application.Core;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;
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

    public IActionResult Index()
    {

        return View();
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

    // [HttpGet("ObtenerComision/{id}")]
    // public async Task<ActionResult<Result<GetVehiculoResponse>>> ObtenerVehiculo(
    //     int id,
    //     CancellationToken cancellationToken
    // )
    // {
    //     var query = new GetVehiculoQueryRequest{Id = id};
    //     var result = await _mediator.Send(query, cancellationToken);
    //     return result.IsSuccess ? View(result.Value) : NotFound(result.Error);
    // }
}