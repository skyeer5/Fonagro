namespace WebApp.Domain;

public class Vehiculo : AuditableEntity
{
    public int VehiculoId { get; set; }
    public string? Placa { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int Anio { get; set; }
    public string? Tipo_Vehiculo { get; set; }
    public string? Color { get; set; }
    public string? Cilindraje { get; set; }
    public double Kilometraje { get; set; }
    public string? Estado { get; set; }
    public Gasolina? Gasolina { get; set; }
    public int? GasolinaId { get; set; }
    public ICollection<Accesorio>? Accesorios { get; set; }
    public ICollection<VehiculoAccesorio>? VehiculoAccesorios { get; set; }
    public ICollection<Parte>? Partes { get; set; }
    public ICollection<VehiculoParte>? VehiculoPartes { get; set; }
    public ICollection<Comision>? Comisiones { get; set; }

    public static Vehiculo Crear(string placa, string marca, string modelo, int anio, string tipo_Vehiculo, string color, string cilindraje, double kilometraje, int? gasolinaId, int creado_Por)
    {
        
        return new Vehiculo
        {
            Placa = placa,
            Marca = marca,
            Modelo = modelo,
            Anio = anio,
            Tipo_Vehiculo = tipo_Vehiculo,
            Color = color,
            Cilindraje = cilindraje,
            Kilometraje = kilometraje,
            GasolinaId = gasolinaId,
            Estado = EstadosTipos.Disponible,
            Fecha_Creacion = DateTime.Now,
            Creado_Por = creado_Por
        };
    }
    public void AgregarListaAccesorios(List<int> accesorios)
    {
        this.VehiculoAccesorios ??= new List<VehiculoAccesorio>();
        foreach (var accesorio in accesorios)
        {
            var vehiculoAccesorio = VehiculoAccesorio.AsignarAVehiculo(accesorio);
            VehiculoAccesorios.Add(vehiculoAccesorio);
        }
    }
    public void AgregarListaPartes(List<Parte> partes)
    {
        VehiculoPartes ??= new List<VehiculoParte>();
        foreach (var parte in partes)
        {
            VehiculoPartes.Add(new VehiculoParte
            {
                Vehiculo = this,
                Parte = parte
            });
        }
    }
    public void ModificarEstadoEnComision()
    {
        Estado = EstadosTipos.Ocupado;
    }
}