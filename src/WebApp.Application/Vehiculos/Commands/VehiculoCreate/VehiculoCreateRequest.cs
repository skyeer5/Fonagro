
using WebApp.Domain.Vehiculos;

namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

    public class VehiculoCreateRequest
    {
        public string? Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Anio { get; set; }
        public VehiculoTipos Tipo_Vehiculo { get; set; }
        public string? Color { get; set; }
        public VehiculoCilindrajes Cilindraje { get; set; }
        public int GasolinaId { get; set; }   
    }
