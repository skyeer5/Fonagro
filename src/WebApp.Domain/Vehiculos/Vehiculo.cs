namespace WebApp.Domain.Vehiculos;
using WebApp.Domain.Comisiones;
using WebApp.Domain.Gasolinas;

public class Vehiculo : AuditableEntity
{
    public int VehiculoId { get; set; }
    public string? Placa { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int Anio { get; set; }
    public string? Tipo_Vehiculo { get; set; }
    public string? Color { get; set; }
    public string? Cilindraje { get; set;}
    public int ConsumoKmPorGalon { get; set; }
    public string? Estado { get; set; }
    public Gasolina? Gasolina { get; set; }
    public int? GasolinaId { get; set; }
    public ICollection<Comision>? Comisiones { get; set; }

    public static Vehiculo Crear(string placa, string marca, string modelo, int anio, string tipo_Vehiculo, string color, string cilindraje, double kilometraje, int? gasolinaId, string tipo_gasolina)
    {
        int consumo = 0;
        if(tipo_gasolina == GasolinaTipos.Disel)
        {
            switch(cilindraje)
            {
                case VehiculoCilindraje.Cilindraje_4:
                    consumo = 32;
                    break;
                case VehiculoCilindraje.Cilindraje_6:
                    consumo = 28;
                    break;
                case VehiculoCilindraje.Cilindraje_8:
                    consumo = 16;
                    break;
                case VehiculoCilindraje.Cilindraje_8_Lujo:
                    consumo = 12;
                    break;
            }
        }
        else
        {
            switch(cilindraje)
            {
                case VehiculoCilindraje.Cilindraje_4:
                    consumo = 35;
                    break;
                case VehiculoCilindraje.Cilindraje_6:
                    consumo = 25;
                    break;
                case VehiculoCilindraje.Cilindraje_8:
                    consumo = 15;
                    break;
                case VehiculoCilindraje.Cilindraje_8_Lujo:
                    consumo = 10;
                    break;
            }
        }
        return new Vehiculo
        {
            Placa = placa,
            Marca = marca,
            Modelo = modelo,
            Anio = anio,
            Tipo_Vehiculo = tipo_Vehiculo,
            Color = color,
            Cilindraje = cilindraje,
            ConsumoKmPorGalon = consumo,
            GasolinaId = gasolinaId,
            Estado = VehiculoEstados.Disponible,
            CreatedDate = DateTime.Now
    };
    }
    public void AgregarCreadoPor(int usuarioId)
    {
        CreatedBy = usuarioId;
    }   
    public void ModificarEstadoEnComision()
    {
        Estado = VehiculoEstados.Ocupado;
    }
    public void ModificarEstadoDisponible()
    {
        Estado = VehiculoEstados.Disponible;
    }
}