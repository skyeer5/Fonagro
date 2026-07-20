namespace WebApp.Domain.Usuarios;

public static class UsuarioEstados
{
    public const string PendientePrimerAcceso = "Pendiente Primer Acceso";
    public const string Activo = "Activo";
    public const string Suspendido = "Suspendido";
    public const string Baja = "Baja";
    public const string EnComision = "En comisión";
    public static List<string> GetEstados()
    {
        return new List<string>
        {
            PendientePrimerAcceso,
            Activo,
            Suspendido,
            EnComision,
            Baja
        };
    }
}