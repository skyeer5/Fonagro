using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Usuarios;

public enum UsuarioEstados
{
    [Display(Name = "Pendiente Primer Acceso")]
    PendientePrimerAcceso = 1,
    [Display(Name = "Activo")]
    Activo = 2,
    [Display(Name = "Suspendido")]
    Suspendido = 3,
    [Display(Name = "Baja")]
    Baja = 4,
    [Display(Name = "En Comisión")]
    EnComision = 5,

}