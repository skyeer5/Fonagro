using WebApp.Application.Comisiones.Queries.GetComisionesActivas;

namespace WebApp.Web.Models.Comisiones;

public class ComisionViewModel
{
    public GetComisionActivaResponse? Comision { get; set; }
    public bool TieneComisionCreada { get; set; } = false;
    public bool TieneDestinosDefinidos { get; set; } = false;
    public bool TieneCombustiblesAprobados { get; set; } = false;
    public bool TieneComisionLista { get; set; } = false;
    public bool TieneComisionEnCurso { get; set; } = false;
    public bool EsPiloto { get; set; } = false;
}