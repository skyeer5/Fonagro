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

    public const string Programada = nameof(Programada);
    public const string EnCurso = nameof(EnCurso);
    public const string Completada = nameof(Completada);
    public const string Cancelada = nameof(Cancelada);
    public const string Pendiente_De_Aprobacion = nameof(Pendiente_De_Aprobacion);

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