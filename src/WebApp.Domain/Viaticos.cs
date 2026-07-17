namespace WebApp.Domain;
using WebApp.Domain.ComisionesViaticos;

public class Viatico : AuditableEntity
{
    public int ViaticoId { get; set; }
    public string? Nombre { get; set; }
    public decimal Monto { get; set; }
    public bool? Vigente { get; set; }
    public ICollection<ComisionViaticos>? ComisionViaticos { get; set; }

    public Viatico()
    {
        
    }
    public Viatico(int id, string? nombre, decimal monto)
    {
        ViaticoId = id;
        Nombre = nombre;
        Monto = monto;
    }
}