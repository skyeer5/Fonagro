namespace WebApp.Domain;

public static class EstadosTipos
{
    // Estados de Vehículo
    public const string Disponible = nameof(Disponible);
    public const string Baja = nameof(Baja);
    public const string EnMantenimiento = nameof(EnMantenimiento);
    public const string Ocupado = nameof(Ocupado);
    public const string Reservado = nameof(Reservado);

    // Estados de ComisionUsuario
    public const string Asignado = nameof(Asignado);
    public const string Finalizado = nameof(Finalizado);
    public const string Cancelada = nameof(Cancelada);
    public const string Completada = nameof(Completada); // cuando termina la comison


    // EStados de Partes
    public const string Buen_Estado = "Buen Estado";
    public const string Rayon = nameof(Rayon);
    public const string Hundimiento = nameof(Hundimiento);
    public const string Golpe = nameof(Golpe);
    public const string Otro = nameof(Otro);

    public static List<string> GetEstadosVehiculo()
    {
        return
        [
            Disponible,
            Baja,
            EnMantenimiento,
            Ocupado,
            Reservado
        ];
    }

}