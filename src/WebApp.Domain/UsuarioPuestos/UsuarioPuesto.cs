using WebApp.Domain.Nombramientos;
using WebApp.Domain.Puestos;

namespace WebApp.Domain.UsuarioPuestos;
public class UsuarioPuesto : AuditableEntity
{
    public int UsuarioPuestoId { get; set; }
    public int UsuarioId { get; set; }
    public int PuestoId { get; set; }
    public Puesto? Puesto { get; set; }
    public DateTime Fecha_Asignacion { get; set; }
    public DateTime? Fecha_Desasignacion { get; set; }
    public ICollection<Nombramiento>? Nombramientos { get; set; }
}