namespace WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

public class GetComisionesDetalleResponse
{
    public int Id { get; set; }
    public string? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public string? Estado { get; set; }
    public string? Descripcion_Vehiculo { get; set; }
}