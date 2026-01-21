namespace WebApp.Domain;

public class ComisionViaticos
{
    public int ComisionViaticosId { get; set; }
    public string? Estado { get; set; }
    public int Cantidad { get; set; }
    public decimal Monto_Unitario_Usado { get; set; }
    public decimal Monto_Total { get; set; }
    public int ComisionId { get; set; }
    public int UsuarioId { get; set; }
    public ComisionUsuario? ComisionUsuario { get; set; }
    public int ViaticoId { get; set; }
    public Viatico? Viatico { get; set; }
}