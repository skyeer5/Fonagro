using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;
using WebApp.Domain;

namespace WebApp.Application.Comisiones.Queries.PlanViajeExcel;

public class PlanViajeResponse
{
    public string? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public string? Descripcion { get; set; }
    public List<GetComisionViaticoResponse>? Viaticos { get; set; }
    public decimal TotalDesayuno => Viaticos?.Where(v => v.Tipo_viatico == ViaticosTipos.Desayuno).Sum(v => v.Monto) ?? 0;
    public decimal TotalAlmuerzo => Viaticos?.Where(v => v.Tipo_viatico == ViaticosTipos.Almuerzo).Sum(v => v.Monto) ?? 0;
    public decimal TotalCena => Viaticos?.Where(v => v.Tipo_viatico == ViaticosTipos.Cena).Sum(v => v.Monto) ?? 0;
    public decimal TotalHospedaje => Viaticos?.Where(v => v.Tipo_viatico == ViaticosTipos.Hospedaje).Sum(v => v.Monto) ?? 0;
    public decimal TotalViaticos => TotalDesayuno + TotalAlmuerzo + TotalCena + TotalHospedaje;
    public List<GetComisionDestinosResponse>? Destinos { get; set; }
    public decimal Precio_Galon { get; set;}
    public decimal TotalCombustibleAutorizado { get; set; }
    public string? Nombre { get; set; }

    public static PlanViajeResponse Crear(string? departamento, DateTime fechaSalida, DateTime fechaRegreso, string? descripcion, List<GetComisionViaticoResponse>? viaticos, List<GetComisionDestinosResponse>? destinos, decimal precioGalon, decimal totalCombustibleAutorizado, string? nombre)
    {
        return new PlanViajeResponse
        {
            Departamento = departamento,
            Fecha_Salida = fechaSalida,
            Fecha_Regreso = fechaRegreso,
            Descripcion = descripcion,
            Viaticos = viaticos,
            Destinos = destinos,
            Precio_Galon = precioGalon,
            TotalCombustibleAutorizado = totalCombustibleAutorizado,
            Nombre = nombre
        };
    }
}