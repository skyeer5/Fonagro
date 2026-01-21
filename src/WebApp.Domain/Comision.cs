
namespace WebApp.Domain;

public class Comision : AuditableEntity
{
    public int ComisionId { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public decimal Precio_Galon_Usado { get; set; }
    public decimal Galon_Estimado { get; set; }
    public decimal Presupuesto_Combustible_Estimado { get; set; }
    public decimal Presupuesto_Combustible_Aprobado { get; set; }
    public string? Estado { get; set; }
    public int UsuarioId { get; set; }
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public ICollection<ComisionDestino>? ComisionDestinos { get; set; }
    public ICollection<ComisionUsuario>? ComisionUsuarios { get; set; }
}