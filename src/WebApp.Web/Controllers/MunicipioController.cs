using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Interfaces;

namespace WebApp.Web.Controllers;
[Authorize]
public class MunicipioController : Controller
{
    private readonly IMunicipioService _municipioService;

    public MunicipioController(IMunicipioService municipioService)
    {
        _municipioService = municipioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMunicipiosByDepartamentoId(int departamentoId)
    {
        var municipios = await _municipioService.GetMunicipiosByDepAsync(departamentoId);
        return Json(municipios);
    }
}