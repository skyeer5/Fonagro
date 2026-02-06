namespace WebApp.Domain;

public class VehiculoParte
{
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public int ParteId { get; set; }
    public Parte? Parte { get; set; }
    public string? Comentario { get; set; }
    public bool Activo { get; set; }

    public static VehiculoParte AsignarAVehiculo(int parteId, string? comentario = null)
    {
        return new VehiculoParte
        {
            ParteId = parteId,
            Comentario = comentario,
            Activo = true
        };
    }
}