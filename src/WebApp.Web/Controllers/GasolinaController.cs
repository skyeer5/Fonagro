using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.GasolinaPrecios.Command.GasolinaPrecioCreate;
using WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;

namespace WebApp.Web.Controllers;
[Authorize]
public class GasolinaController : Controller
{
    private readonly IMediator _mediator;

    public GasolinaController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> UpdatePrecio()
    {
        var command = new GetGasolinasWithPrecioQuery.GetGasolinasWithPrecioQueryRequest();
        var result = await _mediator.Send(command);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        ViewBag.Gasolinas = result.Value;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> UpdatePrecio(
        [FromForm] GasolinaPrecioCreateRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new GasolinaPrecioCreateCommand.GasolinaPrecioCreateCommandRequest(request);
        var result = await _mediator.Send(command, cancellationToken);
        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return RedirectToAction("Index","Comision");
    }
}