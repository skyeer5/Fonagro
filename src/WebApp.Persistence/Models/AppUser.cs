using Microsoft.AspNetCore.Identity;
using WebApp.Domain;
using WebApp.Domain.Usuarios;
using WebApp.Domain.Comisiones;
using WebApp.Domain.Nombramientos;

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
    public ICollection<Nombramiento>? Nombramientos { get; set; }

    public static AppUser Crear(string nombreCompleto, string nit, string puesto, string unidad, string tipoServicios, string numeroContrato, string email)
    {
        return new AppUser
        {
            Nombre_Completo = nombreCompleto.ToUpper(),
            NIT = nit.ToUpper().Replace("-", "").Replace(" ", ""),
            Puesto = DefinirPuesto(puesto, unidad),
            Unidad = unidad.ToUpper(),
            Tipo_Servicios = tipoServicios.ToUpper(),
            Numero_Contrato = numeroContrato,
            Estado = UsuarioEstados.PendientePrimerAcceso,
            Email = email,
            UserName = nit.ToUpper().Replace("-", "").Replace(" ", "")
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