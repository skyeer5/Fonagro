namespace WebApp.Domain;

public class ComisionViaticos
{
    public int ComisionViaticosId { get; set; }
    public string? Estado { get; set; }
    public int Cantidad { get; set; }
    public decimal Monto_Unitario_Usado { get; set; }
    public decimal Monto_Total { get; set; }
    public DateTime Fecha {get;set;} 
    public int ComisionId { get; set; }
    public int UsuarioId { get; set; }
    public ComisionUsuario? ComisionUsuario { get; set; }
    public int ViaticoId { get; set; }
    public Viatico? Viatico { get; set; }

    public static ComisionViaticos Crear(int cantidad, decimal precio_usado, DateTime fecha, int viaticoId, int comisionId)
    {
        return new ComisionViaticos
        {
            Cantidad = cantidad,
            Monto_Unitario_Usado = precio_usado,
            Monto_Total = precio_usado * cantidad,
            Fecha = fecha,
            Estado = ViaticosTipos.Asignado,
            ComisionId = comisionId,
            ViaticoId = viaticoId
        };
    }
    public void AsignarUsuario(int usuarioId)
    {
        this.UsuarioId = usuarioId;
    }
}