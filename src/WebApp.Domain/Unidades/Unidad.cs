using WebApp.Domain.Puestos;

namespace WebApp.Domain.Unidades;

public sealed class Unidad
{
    public int UnidadId { get; set; }
    public string? Nombre { get; set; }
    public ICollection<Puesto>? Puestos { get; set; }
}