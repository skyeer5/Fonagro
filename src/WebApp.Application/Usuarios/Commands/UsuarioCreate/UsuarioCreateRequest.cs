namespace WebApp.Application.Usuarios.Commands.UsuarioCreate;

    public class UsuarioCreateRequest
    {
        public string? Nombre_Completo { get; set; }
        public string? NIT { get; set; }
        public string? Puesto { get; set; }
        public string? Unidad { get; set; }
        public string? Tipo_Servicios { get; set; }
        public string? Numero_Contrato { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int Creado_Por { get; set; }          
    }