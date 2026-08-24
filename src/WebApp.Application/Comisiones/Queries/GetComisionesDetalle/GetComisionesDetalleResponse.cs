namespace WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

public class GetComisionesDetalleResponse
{
    public int ComisionId { get; set; }
    public List<string> DepartamentosYMunicipios { get; set; } = [];
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Descripcion_Vehiculo { get; set; } = string.Empty;
}