namespace WebApp.Domain;

public abstract class AuditableEntity 
{
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}