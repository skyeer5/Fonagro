namespace WebApp.Domain;

public class ComisionUsuario : AuditableEntity
{
    public int ComisionId { get; set; }
    public string? Nombramiento { get; set; }
    public string? Descripcion { get; set; }
    public bool Es_Piloto { get; set; }
    public string? Estado { get; set; }
    public Comision? Comision { get; set; }
    public int UsuarioId { get; set; }
    public ICollection<ComisionViaticos>? ComisionViaticos { get; set; }

    public static ComisionUsuario AsignarAComision(int usuarioId, string nombramiento, bool es_Piloto)
    {
        return new ComisionUsuario
        {
            Nombramiento = nombramiento,
            Es_Piloto = es_Piloto,
            Estado = EstadosTipos.Asignado,
            UsuarioId = usuarioId
        };
    }
};
