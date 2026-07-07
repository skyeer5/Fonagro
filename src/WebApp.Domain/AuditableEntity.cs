namespace WebApp.Domain;

public abstract class AuditableEntity 
{
    public int? Creado_Por { get; set; }
    public DateTime? Fecha_Creacion { get; set; }
}