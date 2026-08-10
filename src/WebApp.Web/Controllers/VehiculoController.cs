using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Core;
using WebApp.Application.Vehiculos.Commands.VehiculoCreate;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
using WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;
using WebApp.Domain.Usuarios;
using WebApp.Domain.Vehiculos;
using WebApp.Web.Extensions;
using WebApp.Web.Models.Vehiculos;
using static WebApp.Application.Gasolinas.Queries.GetGasolinas.GetGasolinasQuery;
using static WebApp.Application.Vehiculos.Commands.VehiculoCreate.VehiculoCreateCommand;
using static WebApp.Application.Vehiculos.Queries.GetVehiculo.GetVehiculoQuery;
using static WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle.GetVehiculosDetalleQuery;

namespace WebApp.Web.Controllers;
[Authorize]
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
    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var gasolinas = await _mediator.Send(new GetGasolinasQueryRequest());

        var model = new VehiculoCreateViewModel
        {
            Gasolinas = gasolinas.Value!.ToSelectList(
                x=> x.Id.ToString(),
                x=>x.Nombre
            ),
            Tipos = EnumExtensions.ToSelectList<VehiculoTipos>(),
            Cilindrajes = EnumExtensions.ToSelectList<VehiculoCilindrajes>()
        };

        return View(model);
    }
    [HttpPost]
    public async Task<ActionResult<Result<int>>> Crear(
        [FromForm] VehiculoCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new VehiculoCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction(nameof(Detalle), new { id = result.Value }) : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<ActionResult<Result<GetVehiculoResponse>>> Detalle(
        int id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetVehiculoQueryRequest{Id = id};
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? View(result.Value) : NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> List(string? marca = "", string? modelo = "", string? placa = "", string? estado = "", int currentPage = 1, string orderBy = "")
    {
        var request = new GetVehiculosDetalleRequest
        {
            Marca = marca,
            Modelo = modelo,
            Placa = placa,
            Estado = estado,
            PageNumber = currentPage,
            OrderBy = orderBy
        };
        var query = new GetVehiculosDetalleQueryRequest(request);
        var result = await _mediator.Send(query);
        return View(result.Value);
    }
}