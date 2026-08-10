using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Vehiculos.Commands.VehiculoCreate;

namespace WebApp.Web.Models.Vehiculos;

public class VehiculoCreateViewModel : VehiculoCreateRequest
{
    public List<SelectListItem>? Gasolinas { get; set; } = [];
    public List<SelectListItem>? Cilindrajes { get; set; } = [];
    public List<SelectListItem>? Tipos { get; set; } = [];
}