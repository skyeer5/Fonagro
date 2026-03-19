
namespace WebApp.Domain;

public class Comision : AuditableEntity
{
    public int ComisionId { get; set; }
    public string? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public decimal Precio_Galon_Usado { get; set; }
    public decimal Galon_Estimado { get; set; }
    public decimal Presupuesto_Combustible_Estimado { get; set; }
    public decimal Presupuesto_Combustible_Aprobado { get; set; }
    public string? Estado { get; set; }
    public int UsuarioId { get; set; }
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public ICollection<ComisionDestino>? ComisionDestinos { get; set; }
    public ICollection<ComisionUsuario>? ComisionUsuarios { get; set; } 

    public static Comision Crear(
                                 string departamento,
                                 DateTime fecha_Salida,
                                 DateTime fecha_Regreso,
                                 int vehiculoId,
                                 decimal gasolinaPrecio
                                )
{
        return new Comision
        {
            Departamento = departamento,
            Fecha_Salida = fecha_Salida,
            Fecha_Regreso = fecha_Regreso,
            Precio_Galon_Usado = gasolinaPrecio,
            VehiculoId = vehiculoId,
            Estado = EstadosTipos.Creada
        };
    }
    public void AgregarCreadoPor(int usuarioId)
    {
        this.UsuarioId = usuarioId;
    }
    public void AgregarUsuarios(ICollection<Nombramiento> usuariosNombrados, List<Viatico> viaticos, DateTime salida, DateTime regreso)
    {
        this.ComisionUsuarios ??= new List<ComisionUsuario>();
        if(!usuariosNombrados.Any())
        {
            throw new Exception("Debe asignar al menos un usuario");
        }
        usuariosNombrados = usuariosNombrados.DistinctBy(u => u.UsuariosId).ToList();
        foreach(var usuario in usuariosNombrados)
        {
            var comisionUsuario = ComisionUsuario.AsignarAComision(
                usuario.UsuariosId,
                usuario.Numero_Nombramiento!,
                usuario.Es_Piloto
            );
            comisionUsuario.AsignarViaticos(viaticos, salida, regreso);
            ComisionUsuarios.Add(comisionUsuario);
            
        }
        
    }
    public void AgregarDestinos(List<ComisionDestino> comisionDestinos)
    {
        this.ComisionDestinos ??= new List<ComisionDestino>();
        if(!comisionDestinos.Any())
        {
            throw new Exception("Debe asignar al menos un destino");
        }
        Console.WriteLine("\n\n\n\n");
        foreach(var destino in comisionDestinos)
        {
            this.ComisionDestinos.Add(destino);
            Console.WriteLine(destino.Galones);
        }
        this.Galon_Estimado = comisionDestinos.Sum( x=>x.Galones);
        this.Presupuesto_Combustible_Estimado = decimal.Multiply(Precio_Galon_Usado, Galon_Estimado);
        this.Estado = EstadosTipos.DestinosDefinidos;

    }
    public void AgregarPresupuestoGas(decimal prespuestoGas)
    {
        this.Estado = EstadosTipos.Programada;
        this.Presupuesto_Combustible_Aprobado = prespuestoGas;
    }
}