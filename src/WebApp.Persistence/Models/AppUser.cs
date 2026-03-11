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

    public static AppUser Crear(string nombreCompleto, string nit, string puesto, string unidad, string tipoServicios, string numeroContrato, string email)
    {
        return new AppUser
        {
            Nombre_Completo = nombreCompleto.ToUpper(),
            NIT = nit.ToUpper().Replace("-", "").Replace(" ", ""),
            Puesto = puesto.ToUpper(),
            Unidad = unidad.ToUpper(),
            Tipo_Servicios = tipoServicios.ToUpper(),
            Numero_Contrato = numeroContrato,
            Estado = UsuarioEstados.Activo,
            Email = email,
            UserName = nit.ToUpper().Replace("-", "").Replace(" ", "")
        };

    }
}