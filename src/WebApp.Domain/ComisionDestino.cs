namespace WebApp.Domain;

public class ComisionDestino : AuditableEntity
{
    public int ComisionDestinoId { get; set; }
    public string? Descripcion { get; set; }
    public decimal Kilometros { get; set; }
    public decimal Galones { get; set; }
    public int ComisionId { get; set; }
    public Comision? Comision { get; set; }

    public static ComisionDestino Crear(string descripcion, decimal kilometros, int kmPorGalor)
    {
        return new ComisionDestino
        {
            Descripcion = descripcion,
            Kilometros = kilometros,
            Galones = kilometros/kmPorGalor
        };  
    }
}