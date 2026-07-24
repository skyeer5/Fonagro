namespace WebApp.Domain.Municipios;
using WebApp.Domain.Departamentos;
using WebApp.Domain.NomMunicipios;

public class Municipio
{
    public int MunicipioId { get; set; }
    public string Nombre { get; set; } = null!;
    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; } = null!;
    public ICollection<NomMunicipio>? NomMunicipios { get; set; }
}