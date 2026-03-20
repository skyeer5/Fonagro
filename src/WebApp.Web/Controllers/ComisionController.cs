using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.ComisionCreate;
using WebApp.Application.Comisiones.Command.ComisionAddDescripcion;
using WebApp.Application.Comisiones.Command.ComisionAddDestinos;
using WebApp.Application.Comisiones.Command.ComisionApprovalGas;
using WebApp.Application.Core;
using WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;
using WebApp.Web.Models;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;
using static WebApp.Application.Comisiones.Command.ComisionAddDescripcion.ComisionAddDescripcionQuery;
using static WebApp.Application.Comisiones.Command.ComisionAddDestinos.ComisionAddDestinosCommand;
using static WebApp.Application.Comisiones.Command.ComisionApprovalGas.ComisionApprovalGasCommand;
using static WebApp.Application.Comisiones.Queries.GetComisionesActivas.GetComisionesActivasQuery;
using static WebApp.Application.Comisiones.Queries.PlanViajeExcel.PlanViajeQuery;
using static WebApp.Application.Gasolinas.Queries.GetGasolinasWithFecha.GetGasolinasWithFechaQuery;
using static WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio.GetGasolinasWithPrecioQuery;
using static WebApp.Application.Usuarios.Queries.GetUsuariosSinComision.GetUsuariosSinComisionQuery;
using static WebApp.Application.Vehiculos.Queries.GetVehiculosDisponibles.GetVehiculosDisponiblesQuery;

namespace WebApp.Web.Controllers;

[Authorize]
[Route("comision")]
public class ComisionController : Controller
{
    private readonly IMediator _mediator;
    public ComisionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("")]
    public async Task<ActionResult<ComisionViewModel>> Index()
    {
        var gasolinas = await _mediator.Send(new GetGasolinasWithFechaQueryRequest());
        ViewBag.Gasolinas = gasolinas.Value;
        foreach(var gasolina in gasolinas.Value!)
        {
            Console.WriteLine($"\n\n\n\n hola we soy {gasolina.Descripcion}");
        }
        var resultado = await _mediator.Send(new GetComisionActivaQueryRequest());

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
        if(resultado.Value!.Prespuesto_Aprobado)
        {
            vm.TieneCombustiblesAprobados = true;
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
    [HttpGet("AgregarGasolina")]
    public IActionResult AgregarGasolina()
    {
        return View();
    }
    [HttpPost("AgregarGasolina")]
    public async Task<ActionResult<Result<int>>> AgregarGasolina(
        [FromForm] ComisionApprovalGasRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ComisionApprovalGasCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction("ObtenerComision", new { id = result.Value }) : BadRequest(result.Error);
    }
    [HttpPost("AgregarDescripcion")]
    public async Task<ActionResult<Result<int>>> AgregarDescripcion(
        [FromForm] ComisionAddDescripcionRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ComisionAddDescripcionCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction("ObtenerComision", new { id = result.Value }) : BadRequest(result.Error);
    }
    [HttpGet("ImprimirPlanViaje/{idUsuario}/{idComision}")]
    public async Task<IActionResult> ImprimirPlanViaje(int idUsuario, int idComision)
    {
        var query = new PlanViajeQueryRequest(idUsuario, idComision);
        var result = await _mediator.Send(query);
        if (!result.Any())
        {
            return NotFound("Plan de viaje no encontrado");
        }
        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"PlanViaje_{idComision}.xlsx");
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