using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Nombramientos.Command.NombramientoApprove;
using WebApp.Application.Nombramientos.Command.NombramientoCreate;
using WebApp.Application.Nombramientos.Queries.GetNombramientos;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Unidades;
using WebApp.Web.Extensions;
using WebApp.Web.Models.Nombramientos;
using static WebApp.Application.Departamentos.Queries.GetDepartamentos.GetDepartamentosQuery;
using static WebApp.Application.Municipios.Queries.GetMunicipiosByDep.GetMunicipiosByDepQuery;
using static WebApp.Application.Nombramientos.Command.NombramientoApprove.NombramientoApproveCommand;
using static WebApp.Application.Nombramientos.Command.NombramientoCreate.NombramientoCreateCommand;
using static WebApp.Application.Nombramientos.Queries.GetNombramientoById.GetNombramientoByIdQuery;
using static WebApp.Application.Nombramientos.Queries.GetNombramientos.GetNomParaAprobarQuery;
using static WebApp.Application.Nombramientos.Queries.GetNomDatosById.GetNomDatosByIdQuery;
using static WebApp.Application.Nombramientos.Queries.NombramientoPdf.NombramientoPdfQuery;
using static WebApp.Application.Usuarios.Queries.GetUsuarios.GetUsuariosQuery;
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
            Fecha_Salida = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(1)),
            Fecha_Regreso = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(2))
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
    public async Task<IActionResult> List(int? usuario, int? unidad, int? correlativo, DateOnly? fecha_inicio, DateOnly? fecha_fin, int? estado, int currentPage = 1, string orderBy = "")
    {
        var request = new GetNombramientosRequest
        {
            Usuario = usuario,
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

        if(!result.IsSuccess)
            TempData["msg"] = result.Error;
        
        var departamentos = await _mediator.Send(new GetDepartamentosQueryRequest());
        if(!departamentos.IsSuccess)
            TempData["msg"] = result.Error;

        var usuarios = await _mediator.Send(new GetUsuariosQueryRequest());
        if(!usuarios.IsSuccess)
            TempData["msg"] = result.Error;

        var model = new NombramientoListViewModel
        {
            Usuario = usuario,
            Correlativo = correlativo,
            Fecha_Inicio = fecha_inicio,
            Fecha_Fin = fecha_fin,
            Estado = estado,
            Unidad = unidad,
            CurrentPage = result.Value?.CurrentPage ?? 1,
            TotalPages = result.Value?.TotalPages ?? 0,
            NombramientosList = result.Value?.Items ?? [],
            UsuariosList = usuarios.Value!.ToSelectList(
                x => x.UsuarioId.ToString(),
                x => x.Nombre_Completo!
            ),
            EstadoList = EnumExtensions.ToSelectList<NombramientoEstados>(),
            UnidadList = EnumExtensions.ToSelectList<UnidadesEnum>()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> VerDetalle(int nombramientoId)
    {
        var query = new GetNombramientoByIdQueryRequest(nombramientoId);
        var result = await _mediator.Send(query);
        
        if(!result.IsSuccess)
        {
            TempData["msg"] = result.Error;
            return RedirectToAction(nameof(List));
        }
        var nombramiento = result.Value;
        var departamentos = await _mediator.Send(new GetDepartamentosQueryRequest());
        var municipios = await _mediator.Send(new GetMunicipiosByDepQueryRequest(result.Value!.Departamentos));

        var model = new NombramientoVerDetalleViewModel
        {
            NombramientoId = nombramiento!.NombramientoId,
            Proposito = nombramiento.Proposito,
            Municipios = nombramiento.Municipios,
            Fecha_Salida = nombramiento.Fecha_Salida,
            Fecha_Regreso = nombramiento.Fecha_Regreso,

            Nombre_Completo = nombramiento.Nombre_Completo,
            Puesto = nombramiento.Puesto,
            Unidad = nombramiento.Unidad,
            Correlativo = nombramiento.Correlativo,
            NombramientoEstado = nombramiento.NombramientoEstado,
            ComisionId = nombramiento.ComisionId,
            ComisionEstado = nombramiento.ComisionEstado,

            DepartamentoList = departamentos.Value!.ToSelectList(
                x => x.Id.ToString(),
                x => x.Nombre!,
                nombramiento.Departamentos
            ),
            MunicipioList = municipios.Value!.ToSelectList(
                x => x.Id.ToString(),
                x => x.Nombre!,
                nombramiento.Municipios
            )
        };

        return View(model);
    }
    
    [HttpPost]
    public async Task<ActionResult> Aprobar(
        [FromForm] NombramientoApproveRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new NombramientoApproveCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        if(!result.IsSuccess)
        {
            TempData["msg"] = result.Error;
        }
        TempData["SuccessMsg"] = "El nombramiento se ha aprobado y registrado con exito!";
        return RedirectToAction(nameof(List));
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