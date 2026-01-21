namespace WebApp.Domain;

public class VehiculoAccesorio
{
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }

    public int AccesorioId { get; set; }
    public Accesorio? Accesorio { get; set; }
    public string? Comentario { get; set; }
    public bool Activo { get; set; }
}