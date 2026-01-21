namespace WebApp.Domain;

public class Viatico : AuditableEntity
{
    public int ViaticoId { get; set; }
    public string? Nombre { get; set; }
    public decimal? Monto { get; set; }
    public bool? Vigente { get; set; }
    public ICollection<ComisionViaticos>? ComisionViaticos { get; set; }
}