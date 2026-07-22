namespace WebApp.Domain.Departamentos;
using WebApp.Domain.Municipios;
public class Departamento
{
    public int DepartamentoId { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<Municipio> Municipios { get; set; } = null!;

    public static List<Departamento> JsonToDepartamento(List<DepartamentoJson> jsonDepartamentos)
    {
        var departamentos = new List<Departamento>();

        foreach (var jsonDepartamento in jsonDepartamentos)
        {
            var departamento = new Departamento
            {
                Nombre = jsonDepartamento.title ?? string.Empty,
                Municipios = new List<Municipio>()
            };

            if (jsonDepartamento.mun != null)
            {
                foreach (var municipioNombre in jsonDepartamento.mun)
                {
                    var municipio = new Municipio
                    {
                        Nombre = municipioNombre,
                        Departamento = departamento
                    };
                    departamento.Municipios.Add(municipio);
                }
            }

            departamentos.Add(departamento);
        }

        return departamentos;
        
    }
}