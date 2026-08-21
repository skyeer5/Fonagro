using System.Security.Policy;

namespace WebApp.Application.Comisiones.Queries.GetComisionesExcel;

public class GetComisionesExcelDto
{
    public int ComisionId { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public DateTime Fecha_Creacion_Comision { get; set; }
    public List<string> DepartamentosYMunicipios { get; set; } = [];
    public List<string> Destinos { get; set; } = [];
    public string Vehiculo { get; set; } = string.Empty;
    public List<string> Nombrados { get; set; } = [];
    public string NombreResponsableVehiculo { get; set; } = string.Empty;
    public string NombreCreadorComision { get; set; } = string.Empty;
    public string NombreAprobadorCombustible { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
    public decimal PresupuestoCombustibleEstimado { get; set; } = 0;
    public decimal PresupuestoCombustibleAprobado { get; set; } = 0;
    public decimal PrecioCombustible { get; set; } = 0;
}