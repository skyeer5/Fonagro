using WebApp.Domain.Usuarios;

namespace WebApp.Application.Usuarios.Commands.UsuarioCreate;

    public class UsuarioCreateRequest
    {
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? NIT { get; set; }
        public int Puesto { get; set; }
        public TipoServicios Tipo_Servicios { get; set; }
        public string? Numero_Contrato { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }