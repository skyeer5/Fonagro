using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Web.Extensions;
using WebApp.Web.Models.Nombramientos;
using static WebApp.Application.Departamentos.Queries.GetDepartamentos.GetDepartamentosQuery;
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
            )
        };
        return View(model);
    }
}