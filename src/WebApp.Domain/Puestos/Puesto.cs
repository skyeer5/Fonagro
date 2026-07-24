using WebApp.Domain.Unidades;
using WebApp.Domain.UsuarioPuestos;

namespace WebApp.Domain.Puestos;
public sealed class Puesto
{
    public int PuestoId { get; set; }
    public string? Nombre { get; set; }
    public int UnidadId { get; set; }
    public Unidad? Unidad { get; set; }
    public ICollection<UsuarioPuesto>? UsuarioPuestos { get; set; }
}