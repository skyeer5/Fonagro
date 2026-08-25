using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WebApp.Application.Municipios.Queries.GetMunicipiosByDep.GetMunicipiosByDepQuery;

namespace WebApp.Web.Controllers;
[Authorize]
public class MunicipioController : Controller
{
    private readonly IMediator _mediator;
    public MunicipioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMunicipiosByDepartamentosIds([FromQuery] List<int> departamentosIds)
    {
        var municipios = await _mediator.Send(new GetMunicipiosByDepQueryRequest(departamentosIds));
        return Json(municipios);
    }
}