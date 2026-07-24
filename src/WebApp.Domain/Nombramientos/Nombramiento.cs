namespace WebApp.Domain.Nombramientos;
using WebApp.Domain.Comisiones;
using WebApp.Domain.ComisionesViaticos;
using WebApp.Domain.NomMunicipios;
using WebApp.Domain.UsuarioPuestos;
using WebApp.Domain.Viaticos;

public class Nombramiento : AuditableEntity
{
    public int NombramientoId { get; set; }
    public int Correlativo { get; set; }
    public string? Proposito { get; set; } // Para Nombramientos de Comisiones
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public string? Descripcion { get; set; } // Para el Plan de Viaje
    public bool Es_Piloto { get; set; }
    public string? Estado { get; set; }
    public int UsuarioPuestoId { get; set; } 
    public UsuarioPuesto? UsuarioPuesto { get; set; }
    public int ComisionId { get; set; }
    public Comision? Comision { get; set; }
    public ICollection<NomMunicipio>? NomMunicipios { get; set; }
    public ICollection<ComisionViaticos>? ComisionViaticosList { get; set; }

    public static Nombramiento AsignarAComision(int usuarioId, string nombramiento, bool es_Piloto)
    {
        return new Nombramiento
        {
            Es_Piloto = es_Piloto,
            Estado = NombramientoTipos.Asignado,
        };
    }
    public void AsignarViaticos(ICollection<Viatico> viaticosVigentes, DateTime salida, DateTime entrada)
    {
        this.ComisionViaticosList ??= new List<ComisionViaticos>();

        var desayuno = viaticosVigentes.FirstOrDefault(x=>x.Nombre == ViaticosTipos.Desayuno);
        var almuerzo = viaticosVigentes.FirstOrDefault(x=>x.Nombre == ViaticosTipos.Almuerzo);
        var cena = viaticosVigentes.FirstOrDefault(x=>x.Nombre== ViaticosTipos.Cena);
        var hospedaje = viaticosVigentes.FirstOrDefault(x=>x.Nombre == ViaticosTipos.Hospedaje);

        var dias = (entrada-salida).Days + 1;
        var diaActual = salida.Date;


        for (int i = 1; i <= dias; i++)
        {
            if (i == 1)
            {
                if (salida.TimeOfDay < TimeSpan.FromHours(11))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, desayuno!.Monto, diaActual, desayuno.ViaticoId, ComisionId)
                        );
                }
                if (salida.TimeOfDay < TimeSpan.FromHours(17))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, almuerzo!.Monto, diaActual, almuerzo.ViaticoId, ComisionId)
                        );
                }
                if (salida.TimeOfDay < TimeSpan.FromHours(24))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, cena!.Monto, diaActual, cena.ViaticoId, ComisionId)
                        );
                }

            }
            else if (i == dias && i != 1)
            {
                if (entrada.TimeOfDay >= TimeSpan.FromHours(6))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, desayuno!.Monto, diaActual, desayuno.ViaticoId, ComisionId)
                        );
                }
                if (entrada.TimeOfDay >= TimeSpan.FromHours(11))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, almuerzo!.Monto, diaActual, almuerzo.ViaticoId, ComisionId)
                        );
                }
                if (entrada.TimeOfDay >= TimeSpan.FromHours(17))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, cena!.Monto, diaActual, cena.ViaticoId, ComisionId)
                        );
                }
            }
            else
            {
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, desayuno!.Monto, diaActual, desayuno.ViaticoId, ComisionId)
                        );
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, almuerzo!.Monto, diaActual, almuerzo.ViaticoId, ComisionId)
                        );
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, cena!.Monto, diaActual, cena.ViaticoId, ComisionId)
                        );
            }

            if(i<dias)
            {
                ComisionViaticosList.Add(
                    ComisionViaticos.Crear(1, hospedaje!.Monto, diaActual, hospedaje.ViaticoId, ComisionId)
                    );
            }

            diaActual = diaActual.AddDays(1);
        }

    }
};
