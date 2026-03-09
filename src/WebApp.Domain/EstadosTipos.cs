namespace WebApp.Domain;

public static class EstadosTipos
{
    // Estados de Vehículo
    public const string Disponible = nameof(Disponible);
    public const string Baja = nameof(Baja);
    public const string EnMantenimiento = nameof(EnMantenimiento);
    public const string Ocupado = nameof(Ocupado);
    public const string Reservado = nameof(Reservado);

    // EStados de Comision

    public const string Creada = nameof(Creada); // Cuando la crea servicios 
    public const string DestinosDefinidos = nameof(DestinosDefinidos); // cuando se ponen los destinos
    public const string Programada = nameof(Programada); // cuando ya queda aprobada y lista para continuar
    public const string EnCurso = nameof(EnCurso); // cuando empieza la comision
    public const string Completada = nameof(Completada); // cuando termina la comison
    public const string Cancelada = nameof(Cancelada); // lo que dice


    // Estados de ComisionUsuario
    public const string Asignado = nameof(Asignado);
    public const string Finalizado = nameof(Finalizado);

    // EStados de Partes
    public const string Buen_Estado = nameof(Buen_Estado);
    public const string Rayon = nameof(Rayon);
    public const string Hundimiento = nameof(Hundimiento);
    public const string Golpe = nameof(Golpe);
    public const string Otro = nameof(Otro);

}