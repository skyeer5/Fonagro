namespace WebApp.Domain.Nombramientos;
using WebApp.Domain.Comisiones;
using WebApp.Domain.ComisionesViaticos;
using WebApp.Domain.enums;
using WebApp.Domain.Viaticos;

public class Nombramiento : AuditableEntity
{
    public int NombramientoId { get; set; }
    public Unidades Unidad { get; set; }
    public int UsuarioId { get; set; }
    public string? Usuario_Unidad { get; set; }
    public string? Usuario_Puesto { get; set; }
    public string? Proposito { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public int ComisionId { get; set; }
    public string? Num_Nombramiento { get; set; }
    public string? Descripcion { get; set; }
    public bool Es_Piloto { get; set; }
    public string? Estado { get; set; }
    public Comision? Comision { get; set; } 
    
    public ICollection<ComisionViaticos>? ComisionViaticosList { get; set; }

    public static Nombramiento AsignarAComision(int usuarioId, string nombramiento, bool es_Piloto)
    {
        return new Nombramiento
        {
            Num_Nombramiento = nombramiento,
            Es_Piloto = es_Piloto,
            Estado = NombramientoTipos.Asignado,
            UsuarioId = usuarioId
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
