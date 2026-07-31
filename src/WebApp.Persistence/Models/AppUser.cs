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

    public static AppUser Crear(string nombres, string apellidos, string nit,  TipoServicios tipoServicios, string numeroContrato, string email, int puestoId)
    {
        var asignacion = AsignacionUsuario.Crear(puestoId);
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
    public static string DefinirPuesto(string puesto, string unidad)
    {
        if(puesto.Contains(UsuariosTipos.AUXILIAR) || puesto.Contains(UsuariosTipos.ENCARGADO))
        {
            return puesto;
        }
        switch(unidad)
        {
            case UsuariosTipos.UA:
                switch(puesto)
                {
                    case UsuariosTipos.ASESOR:
                        return UsuariosTipos.ASESOR_ADMON;
                    case UsuariosTipos.ASISTENTE:
                        return UsuariosTipos.ASISTENTE_ADMON;
                    case UsuariosTipos.COORDINADOR:
                        return UsuariosTipos.COORDINADOR_ADMON;
                    case UsuariosTipos.SUBCOORDINADOR:
                        return UsuariosTipos.SUBCOORDINADOR_ADMON;
                    default:
                        return puesto;
                }
            case UsuariosTipos.UAJ:
                switch(puesto)
                {
                    case UsuariosTipos.ASESOR:
                        return UsuariosTipos.ASESOR_UAJ;
                    case UsuariosTipos.ASISTENTE:
                        return UsuariosTipos.ASISTENTE_UAJ;
                    case UsuariosTipos.COORDINADOR:
                        return UsuariosTipos.COORDINADOR_UAJ;
                    case UsuariosTipos.SUBCOORDINADOR:
                        return UsuariosTipos.SUBCOORDINADOR_UAJ;
                    default:
                        return puesto;
                }
            case UsuariosTipos.UTSE:
                switch(puesto)
                {
                    case UsuariosTipos.ASESOR:
                        return UsuariosTipos.ASESOR_UTSE;
                    case UsuariosTipos.ASISTENTE:
                        return UsuariosTipos.ASISTENTE_UTSE;
                    case UsuariosTipos.COORDINADOR:
                        return UsuariosTipos.COORDINADOR_UTSE;
                    case UsuariosTipos.SUBCOORDINADOR:
                        return UsuariosTipos.SUBCOORDINADOR_UTSE;
                    default:
                        return puesto;
                }
            case UsuariosTipos.UDAI:
                switch(puesto)
                {
                    case UsuariosTipos.ASESOR:
                        return UsuariosTipos.ASESOR_UDAI;
                    case UsuariosTipos.ASISTENTE:
                        return UsuariosTipos.ASISTENTE_UDAI;
                    case UsuariosTipos.COORDINADOR:
                        return UsuariosTipos.COORDINADOR_UDAI;
                    case UsuariosTipos.SUBCOORDINADOR:
                        return UsuariosTipos.SUBCOORDINADOR_UDAI;
                    default:
                        return puesto;
                }



            default:
                return puesto;
        }

    }
}