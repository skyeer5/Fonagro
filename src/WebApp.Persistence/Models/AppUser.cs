using Microsoft.AspNetCore.Identity;
using WebApp.Domain;

namespace WebApp.Persistence.Models;


public class AppUser : IdentityUser<int>
{
    public string? Nombre_Completo { get; set; }
    public string? NIT { get; set; }
    public string? Puesto { get; set; }
    public string? Unidad { get; set; }
    public string? Tipo_Servicios{ get; set; }
    public string? Numero_Contrato { get; set; }
    public string? Estado { get; set; }
    public ICollection<Comision>? Comisiones { get; set; }
    public ICollection<ComisionUsuario>? ComisionUsuarios { get; set; }
}