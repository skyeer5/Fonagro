using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Usuarios;

public enum UsuarioEstados
{
    
    [Display(Name = "Activo")]
    Activo = 1,
    [Display(Name = "Inactivo")]
    Baja = 2,
    [Display(Name = "Pendiente Primer Acceso")]
    PendientePrimerAcceso = 3,
}