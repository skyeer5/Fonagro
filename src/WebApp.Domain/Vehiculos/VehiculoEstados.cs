namespace WebApp.Domain.Vehiculos;

public static class VehiculoEstados
{
    // Estados de Vehículo
    public const string Disponible = nameof(Disponible);
    public const string Baja = nameof(Baja);
    public const string EnMantenimiento = nameof(EnMantenimiento);
    public const string Ocupado = nameof(Ocupado);
    public const string Reservado = nameof(Reservado);

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