using System.Security.Policy;
using WebApp.Domain.Unidades;

namespace WebApp.Application.Comisiones.Queries.GetComisionesExcel;

public class GetComisionesExcelRequest
{
    public DateOnly? Fecha_Salida { get; set; }
    public DateOnly? Fecha_Regreso { get; set; }
    public List<int>? Departamentos { get; set; }
    public List<int>? Municipios { get; set; }
    public int? Vehiculo { get; set; }
    public int? Usuario { get; set; }
    public List<int>? Unidades { get; set; }
    public int? Estado { get; set; }
}