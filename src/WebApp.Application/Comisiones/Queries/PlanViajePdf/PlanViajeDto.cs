using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinosDetail;
using WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;
using WebApp.Domain;

namespace WebApp.Application.Comisiones.Queries.PlanViajePdf;

public class PlanViajeDto
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
    
    public decimal TotalKms => Destinos?.Sum(d => d.Kilometros) ?? 0;
    public decimal TotalGalones => Destinos?.Sum(d => d.Galones) ?? 0;
    public decimal TotalCombustible { get; set; }
    public List<GetComisionDestinosDetailResponse>? Destinos { get; set; }
    public decimal Precio_Galon { get; set;}
    public bool Es_Gasolina { get; set; }
    public decimal TotalCombustibleAutorizado { get; set; }
    public string? Nombre { get; set; }

}