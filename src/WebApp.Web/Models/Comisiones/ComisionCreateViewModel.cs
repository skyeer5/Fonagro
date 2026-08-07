using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.ComisionCreate;

namespace WebApp.Web.Models.Comisiones;

public class ComisionCreateViewModel : ComisionCreateRequest
{
    public List<SelectListItem>? Nombramientos { get; set; } = [];
    public List<SelectListItem>? Vehiculos { get; set; } = [];
}