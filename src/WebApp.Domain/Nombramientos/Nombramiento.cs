namespace WebApp.Domain.Nombramientos;
using WebApp.Domain.Comisiones;
using WebApp.Domain.ComisionesViaticos;
using WebApp.Domain.NomMunicipios;
using WebApp.Domain.AsignacionUsuarios;
using WebApp.Domain.Viaticos;

public class Nombramiento : AuditableEntity
{
    public int NombramientoId { get; set; }
    public int Correlativo { get; set; }
    public string? Proposito { get; set; } // Para Nombramientos de Comisiones
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public string? Descripcion { get; set; } // Para el Plan de Viaje
    public int UsuarioId_Creador { get; set; }
    public DateTime Fecha_Creado { get; set; }
    public int UsuarioId_Aprobador { get; set; }
    public DateTime Fecha_Aprobado { get; set; }
    public NombramientoEstados Estado { get; set; }
    public int AsignacionUsuarioId { get; set; } 
    public AsignacionUsuario? AsignacionUsuario { get; set; }
    public int? ComisionId { get; set; }
    public Comision? Comision { get; set; }
    public ICollection<NomMunicipio>? NomMunicipios { get; set; }
    public ICollection<ComisionViaticos>? ComisionViaticosList { get; set; }

    public static Nombramiento Crear(int usuarioId, string proposito, DateTime fechaSalida, DateTime fechaRegreso, List<int> Municipios, int correlativo, int usuarioId_Creador)
    {
        var municipios = new List<NomMunicipio>();
        foreach (var municipioId in Municipios)
        {
            var nomMunicipio = new NomMunicipio
            {
                MunicipioId = municipioId
            };
            municipios.Add(nomMunicipio);
        }
        return new Nombramiento
        {
            Proposito = proposito,
            Fecha_Salida = fechaSalida,
            Fecha_Regreso = fechaRegreso,
            Estado = NombramientoEstados.Creado,
            AsignacionUsuarioId = usuarioId,
            NomMunicipios = municipios,
            Correlativo = correlativo,
            UsuarioId_Creador = usuarioId_Creador,
            Fecha_Creado = DateTime.Now,
            Creado_Por = usuarioId_Creador,
            Fecha_Creacion = DateTime.Now
        };
    }
    public static Nombramiento AsignarAComision(int usuarioId, string nombramiento, bool es_Piloto)
    {
        return new Nombramiento
        {
            Estado = NombramientoEstados.Creado,
        };
    }

    public void AprobarNombramiento(int usuarioAprobador)
    {
        this.UsuarioId_Aprobador = usuarioAprobador;
        this.Fecha_Aprobado = DateTime.Now;
        this.Estado = NombramientoEstados.Aprobado;
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
                        ComisionViaticos.Crear(1, desayuno!.Monto, diaActual, desayuno.ViaticoId, ComisionId!.Value)
                        );
                }
                if (salida.TimeOfDay < TimeSpan.FromHours(17))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, almuerzo!.Monto, diaActual, almuerzo.ViaticoId, ComisionId!.Value)
                        );
                }
                if (salida.TimeOfDay < TimeSpan.FromHours(24))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, cena!.Monto, diaActual, cena.ViaticoId, ComisionId!.Value)
                        );
                }

            }
            else if (i == dias && i != 1)
            {
                if (entrada.TimeOfDay >= TimeSpan.FromHours(6))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, desayuno!.Monto, diaActual, desayuno.ViaticoId, ComisionId!.Value)
                        );
                }
                if (entrada.TimeOfDay >= TimeSpan.FromHours(11))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, almuerzo!.Monto, diaActual, almuerzo.ViaticoId, ComisionId!.Value)
                        );
                }
                if (entrada.TimeOfDay >= TimeSpan.FromHours(17))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, cena!.Monto, diaActual, cena.ViaticoId, ComisionId!.Value)
                        );
                }
            }
            else
            {
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, desayuno!.Monto, diaActual, desayuno.ViaticoId, ComisionId!.Value)
                        );
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, almuerzo!.Monto, diaActual, almuerzo.ViaticoId, ComisionId!.Value)
                        );
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, cena!.Monto, diaActual, cena.ViaticoId, ComisionId!.Value)
                        );
            }

            if(i<dias)
            {
                ComisionViaticosList.Add(
                    ComisionViaticos.Crear(1, hospedaje!.Monto, diaActual, hospedaje.ViaticoId, ComisionId!.Value)
                    );
            }

            diaActual = diaActual.AddDays(1);
        }

    }
};
