namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

    public class VehiculoCreateRequest
    {
        public string? Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Anio { get; set; }
        public string? Tipo_Vehiculo { get; set; }
        public string? Color { get; set; }
        public string? Cilindraje { get; set; }
        public double Kilometraje { get; set; }
        public int GasolinaId { get; set; }
        public int Creado_Por { get; set; } 
        public List<int> Partes { get; set; } = new();
        public List<int> Accesorios { get; set; } = new();       
    }
