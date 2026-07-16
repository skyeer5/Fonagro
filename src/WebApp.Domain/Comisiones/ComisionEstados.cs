namespace WebApp.Domain.Comisiones;

public static class ComisionEstados
{
    public const string Creada = nameof(Creada); // Cuando la crea servicios 
    public const string DestinosDefinidos = nameof(DestinosDefinidos); // cuando se ponen los destinos
    public const string Programada = nameof(Programada); // cuando ya queda aprobada y lista para continuar
    public const string EnCurso = nameof(EnCurso); // cuando empieza la comision
    public const string Completada = nameof(Completada); // cuando termina la comison
    public const string Cancelada = nameof(Cancelada); // lo que dice

    public static List<string> GetEstadosComision()
    {
        return
        [
            Creada,
            DestinosDefinidos,
            Programada,
            EnCurso,
            Completada,
            Cancelada
        ];
    }
}