namespace WebApp.Domain.Municipios;
using WebApp.Domain.Departamentos;

public class Municipio
{
    public int MunicipioId { get; set; }
    public string Nombre { get; set; } = null!;
    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; } = null!;
}