using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Core;
using WebApp.Application.Puestos.Queries.GetPuestosByUnidad;
using WebApp.Application.Unidades.Queries.GetUnidades;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Domain.Usuarios;
using WebApp.Web.Extensions;
using WebApp.Web.Models.Usuarios;
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
    public async Task<IActionResult> Crear()
    {
        var unidades = await _mediator.Send(new GetUnidadesQuery.GetUnidadesQueryRequest());

        var model = new UsuarioCreateViewModel
        {
            Unidades = unidades.Value!.ToSelectList(
                x=>x.Id.ToString(),
                x=>x.Nombre!
            ),
            TipoServicios = EnumExtensions.ToSelectList<TipoServicios>()
        };

        return View(model);
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<int>>> Crear(
        [FromForm] UsuarioCreateViewModel model,
        CancellationToken cancellationToken
    )
    {
        var request = new UsuarioCreateRequest
        {
            Nombre_Completo = model.Nombre_Completo,
            NIT = model.NIT,
            Numero_Contrato = model.Numero_Contrato,
            Email = model.Email,
            Password = model.Password,
            Tipo_Servicios = model.Tipo_Servicios.ToString(),
            Unidad = model.Unidad.ToString(),
            Puesto = "1"
        };
        var command = new UsuarioCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData["msg"] = result.Error;

            var unidades = await _mediator.Send(new GetUnidadesQuery.GetUnidadesQueryRequest());

            var newModel = new UsuarioCreateViewModel
            {
                Unidades = unidades.Value!.ToSelectList(
                    x=>x.Id.ToString(),
                    x=>x.Nombre!
                ),
                TipoServicios = EnumExtensions.ToSelectList<TipoServicios>()
            };

            return View(newModel);
        }

        return RedirectToAction(nameof(List));
    }

    [HttpGet]
    public async Task<IActionResult> GetPuestosByUnidadId(int unidadId)
    {
        var puestos = await _mediator.Send(new GetPuestosByUnidadQuery.GetPuestosByUnidadQueryRequest(unidadId));
        return Json(puestos.Value);
    }

    [HttpGet]
    public async Task<IActionResult> List(string? nombre = "", string? puesto = "", string? unidad = "", string? estado = "", int currentPage = 1, string orderBy = "")
    {
        ViewBag.Estados = UsuarioEstados.GetEstados();
        var request = new GetUsuariosActivosDetalleRequest
        {
            Nombre = nombre,
            Estado = estado,
            PageNumber = currentPage,
            OrderBy = orderBy
        };
        var query = new GetUsuariosActivosDetalleQueryRequest(request);
        var result = await _mediator.Send(query);
        return View(result.Value);
    }
}