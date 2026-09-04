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
    public int? UsuarioId_Aprobador { get; set; }
    public DateTime? Fecha_Aprobado { get; set; }
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
            CreatedBy = usuarioId_Creador,
            CreatedDate = DateTime.Now
        };
    }

    public static bool TodosTienenMismosDatos(List<Nombramiento> nombramientos)
    {
        if(nombramientos.Count == 1) 
            return true;
        var validarMunicipios = ValidarMunicipiosParaComision(nombramientos);
        if(!validarMunicipios) 
            return false;
        
        var validarHorarios = ValidarHorarioParaComision(nombramientos);
        if(!validarHorarios) 
            return false;
        
        return true;
    }

    private static bool ValidarMunicipiosParaComision(List<Nombramiento> nombramientos)
    {
        var primerGrupoIds = nombramientos[0].NomMunicipios!
            .Select(nm => nm.MunicipioId)
            .ToHashSet();

        return nombramientos.Skip(1).All(n => 
        {
            var idsActuales = n.NomMunicipios!
                .Select(nm => nm.MunicipioId)
                .ToHashSet();

            return primerGrupoIds.SetEquals(idsActuales);
        });
    }

    private static bool ValidarHorarioParaComision(List<Nombramiento> nombramientos)
    {
        var nombramientoPiloto = nombramientos.First();
        foreach(var nom in nombramientos)
        {
            if((nom.Fecha_Salida.Date != nombramientoPiloto.Fecha_Salida.Date) 
                || (nom.Fecha_Regreso.Date != nombramientoPiloto.Fecha_Regreso.Date))
            return false;
        }
        return true;
    }
    public void AprobarNombramiento(int usuarioAprobador)
    {
        this.UsuarioId_Aprobador = usuarioAprobador;
        this.Fecha_Aprobado = DateTime.Now;
        this.Estado = NombramientoEstados.Aprobado;
    }
    public void AsignarViaticos(ICollection<Viatico> viaticosVigentes)
    {
        this.ComisionViaticosList ??= new List<ComisionViaticos>();

        var desayuno = viaticosVigentes.FirstOrDefault(x=>x.Nombre == ViaticosTipos.Desayuno);
        var almuerzo = viaticosVigentes.FirstOrDefault(x=>x.Nombre == ViaticosTipos.Almuerzo);
        var cena = viaticosVigentes.FirstOrDefault(x=>x.Nombre== ViaticosTipos.Cena);
        var hospedaje = viaticosVigentes.FirstOrDefault(x=>x.Nombre == ViaticosTipos.Hospedaje);

        var dias = (Fecha_Regreso-Fecha_Salida).Days + 1;
        var diaActual = Fecha_Salida.Date;


        for (int i = 1; i <= dias; i++)
        {
            if (i == 1)
            {
                if (Fecha_Salida.TimeOfDay < TimeSpan.FromHours(11))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, desayuno!.ViaticoId)
                        );
                }
                if (Fecha_Salida.TimeOfDay < TimeSpan.FromHours(17))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, almuerzo!.ViaticoId)
                        );
                }
                if (Fecha_Salida.TimeOfDay < TimeSpan.FromHours(24))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, cena!.ViaticoId)
                        );
                }
                if(dias>1)
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, hospedaje!.ViaticoId)
                        );
                }

            }
            else if (i == dias && i != 1)
            {
                if (Fecha_Regreso.TimeOfDay >= TimeSpan.FromHours(6))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, desayuno!.ViaticoId)
                        );
                }
                if (Fecha_Regreso.TimeOfDay >= TimeSpan.FromHours(11))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, almuerzo!.ViaticoId)
                        );
                }
                if (Fecha_Regreso.TimeOfDay >= TimeSpan.FromHours(17))
                {
                    ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, cena!.ViaticoId)
                        );
                }
            }
            else
            {
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, desayuno!.ViaticoId)
                        );
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, almuerzo!.ViaticoId)
                        );
                ComisionViaticosList.Add(
                        ComisionViaticos.Crear(1, diaActual, cena!.ViaticoId)
                        );
                ComisionViaticosList.Add(
                    ComisionViaticos.Crear(1, diaActual, hospedaje!.ViaticoId)
                    );
            }

            diaActual = diaActual.AddDays(1);
        }

    }
};
