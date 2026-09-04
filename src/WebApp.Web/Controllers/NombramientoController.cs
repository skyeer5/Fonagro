using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Nombramientos.Command.NombramientoApprove;
using WebApp.Application.Nombramientos.Command.NombramientoCreate;
using WebApp.Application.Nombramientos.Queries.GetNombramientos;
using WebApp.Application.Nombramientos.Queries.NombramientoPdf;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Unidades;
using WebApp.Web.Extensions;
using WebApp.Web.Models.Nombramientos;
using static WebApp.Application.Departamentos.Queries.GetDepartamentos.GetDepartamentosQuery;
using static WebApp.Application.Nombramientos.Command.NombramientoApprove.NombramientoApproveCommand;
using static WebApp.Application.Nombramientos.Command.NombramientoCreate.NombramientoCreateCommand;
using static WebApp.Application.Nombramientos.Queries.GetNombramientos.GetNomParaAprobarQuery;
using static WebApp.Application.Nombramientos.Queries.GetNomDatosById.GetNomDatosByIdQuery;
using static WebApp.Application.Nombramientos.Queries.NombramientoPdf.NombramientoPdfQuery;
using static WebApp.Application.Usuarios.Queries.GetUsuariosSinNom.GetUsuariosSinNomQuery;

namespace WebApp.Web.Controllers;
[Authorize]
public class NombramientoController : Controller
{
    private readonly IMediator _mediator;

    public NombramientoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var usuarios = await _mediator.Send(new GetUsuariosSinNomQueryRequest());
        var departamentos = await _mediator.Send(new GetDepartamentosQueryRequest());
        var model = new NombramientoCreateViewModel
        {
            Usuarios = usuarios.Value!.ToSelectList(
                x => x.UsuarioId.ToString(),
                x => x.Nombre_Completo!
            ),
            Departamentos = departamentos.Value!.ToSelectList(
                x => x.Id.ToString(),
                x => x.Nombre!
            ),
            Fecha_Salida = DateTime.Now.Date,
            Fecha_Regreso = DateTime.Now.Date.AddDays(1)
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromForm] NombramientoCreateViewModel model,
        CancellationToken cancellationToken
    )
    {
        var request = new NombramientoCreateRequest
        {
            UsuarioId = model.UsuarioId,
            Proposito = model.Proposito,
            Fecha_Salida = model.Fecha_Salida,
            Fecha_Regreso = model.Fecha_Regreso,
            Municipios = model.Municipios
        };
        var command = new NombramientoCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        if(!result.IsSuccess)
        {
            TempData["msg"] = result.Error;
            return RedirectToAction(nameof(Crear));
        }
        TempData["SuccessMsg"] = "El nombramiento se ha creado y registrado con exito!";
        return RedirectToAction(nameof(List));
    }

    [HttpGet]
    public async Task<IActionResult> List(string? nombre_nombrado = null, int? unidad = null, int? correlativo = null, DateTime? fecha_inicio = null, DateTime? fecha_fin = null, int? estado = null, int currentPage = 1, string orderBy = "")
    {
        ViewBag.Estados = EnumExtensions.ToSelectList<NombramientoEstados>();
        ViewBag.Unidades = EnumExtensions.ToSelectList<UnidadesEnum>();
        var request = new GetNombramientosRequest
        {
            Nombre_Nombrado = nombre_nombrado,
            Unidad = unidad,
            Correlativo = correlativo,
            Fecha_Inicio = fecha_inicio,
            Fecha_Fin = fecha_fin,
            Estado = estado,
            PageNumber = currentPage,
            OrderBy = orderBy
        };
        var query = new GetNombramientosQueryRequest(request);
        var result = await _mediator.Send(query);
        return View(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult> Aprobar(
        [FromForm] NombramientoApproveRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new NombramientoApproveCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? RedirectToAction(nameof(List)) : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<ActionResult> ObtenerDatosById(int nombramientoId)
    {
        var query = new GetNomDatosByIdQueryRequest(nombramientoId);
        var nombramiento = await _mediator.Send(query);
        if(!nombramiento.IsSuccess)
        {
            return Json(nombramiento.Error);
        }
        return Json(nombramiento.Value);
    }
        [HttpGet]
    public async Task<IActionResult> Imprimir(int nombramientoId)
    {
        var query = new NombramientoPdfQueryRequest(nombramientoId);
        var result = await _mediator.Send(query);
        if (!result.IsSuccess)
        {
            return NotFound("Nombramiento no encontrado");
        }
        return File(result.Value!.Pdf, "application/pdf", $"Nombramiento_{result.Value.Correlativo}.pdf");
    }

}