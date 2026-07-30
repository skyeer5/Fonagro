using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Usuarios;

public enum TipoServicios
{
    [Display(Name = "SERVICIOS TÉCNICOS")]
    TECNICOS = 1,
    [Display(Name = "SERVICIOS PROFESIONALES")]
    PROFESIONALES = 2
}