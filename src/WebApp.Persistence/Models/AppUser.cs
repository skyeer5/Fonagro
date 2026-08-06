using Microsoft.AspNetCore.Identity;
using WebApp.Domain;
using WebApp.Domain.AsignacionUsuarios;
using WebApp.Domain.Usuarios;

namespace WebApp.Persistence.Models;


public class AppUser : IdentityUser<int>
{
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? NIT { get; set; }
    public TipoServicios Tipo_Servicios{ get; set; }
    public string? Numero_Contrato { get; set; }
    public UsuarioEstados Estado { get; set; }
    public ICollection<AsignacionUsuario>? UsuarioPuestos { get; set; } 

    public static AppUser Crear(string nombres, string apellidos, string nit,  TipoServicios tipoServicios, string numeroContrato, string email, int puestoId, int usuarioCreador)
    {
        var asignacion = AsignacionUsuario.Crear(puestoId, usuarioCreador);
        return new AppUser
        {
            Nombres = nombres.ToUpper(),
            Apellidos = apellidos.ToUpper(),
            NIT = nit.ToUpper().Replace("-", "").Replace(" ", ""),
            Tipo_Servicios = tipoServicios,
            Numero_Contrato = numeroContrato,
            Estado = UsuarioEstados.PendientePrimerAcceso,
            Email = email,
            UserName = nit.ToUpper().Replace("-", "").Replace(" ", ""),
            UsuarioPuestos = new List<AsignacionUsuario>{asignacion}
        };

    }

}