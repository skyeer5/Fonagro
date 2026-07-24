using WebApp.Domain.Municipios;
using WebApp.Domain.Nombramientos;

namespace WebApp.Domain.NomMunicipios;

public class NomMunicipio
{
    public int NombramientoId { get; set; }
    public Nombramiento? Nombramiento { get; set; }
    public int MunicipioId { get; set; }
    public Municipio? Municipio { get; set; }

}