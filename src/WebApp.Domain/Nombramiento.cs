namespace WebApp.Domain;

public sealed class Nombramiento
{
    public int UsuariosId { get; set; } 
    public string? Numero_Nombramiento { get; set; }
    public bool Es_Piloto { get; set; }
}