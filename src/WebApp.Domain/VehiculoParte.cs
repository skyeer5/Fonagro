namespace WebApp.Domain;

public class VehiculoParte
{
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public int ParteId { get; set; }
    public Parte? Parte { get; set; }
    public string? Comentario { get; set; }
}