
namespace WebApp.Domain.Comisiones;

public class Comision : AuditableEntity
{
    public int ComisionId { get; set; }
    public string? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public decimal Precio_Galon_Usado { get; set; }
    public decimal Galon_Estimado { get; set; }
    public decimal Presupuesto_Combustible_Estimado { get; set; }
    public int UsuarioId_Aprobador_Combustible { get; set; }
    public decimal Presupuesto_Combustible_Aprobado { get; set; }
    public string? Estado { get; set; }
    public int UsuarioId { get; set; }
    public int VehiculoId { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public ICollection<ComisionDestino>? ComisionDestinos { get; set; }
    public ICollection<ComisionUsuario>? ComisionUsuarios { get; set; } 

    public static Comision Crear(
                                 List<string> departamento,
                                 DateTime fecha_Salida,
                                 DateTime fecha_Regreso,
                                 int vehiculoId,
                                 decimal gasolinaPrecio
                                )
{
        return new Comision
        {
            Departamento = departamento.Any() ? string.Join(", ", departamento.OrderBy(x => x)) : null,
            Fecha_Salida = fecha_Salida,
            Fecha_Regreso = fecha_Regreso,
            Precio_Galon_Usado = gasolinaPrecio,
            VehiculoId = vehiculoId,
            Estado = ComisionEstados.Creada
        };
    }
    public void AgregarCreadoPor(int usuarioId)
    {
        this.UsuarioId = usuarioId;
        this.Creado_Por = usuarioId;
        this.Fecha_Creacion = DateTime.Now;
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
    public void AgregarDestinos(List<ComisionDestino> comisionDestinos, int userId)
    {
        this.ComisionDestinos ??= new List<ComisionDestino>();
        if(!comisionDestinos.Any())
        {
            throw new Exception("Debe asignar al menos un destino");
        }
        foreach(var destino in comisionDestinos)
        {
            destino.Creado_Por = userId;
            destino.Fecha_Creacion = DateTime.Now;
            this.ComisionDestinos.Add(destino);
        }
        this.Galon_Estimado = comisionDestinos.Sum( x=>x.Galones);
        this.Presupuesto_Combustible_Estimado = decimal.Multiply(Precio_Galon_Usado, Galon_Estimado);
        this.Estado = ComisionEstados.DestinosDefinidos;

    }
    public void AgregarPresupuestoGas(decimal prespuestoGas)
    {
        this.Estado = ComisionEstados.Programada;
        this.Presupuesto_Combustible_Aprobado = prespuestoGas;
    }
    public void AgregarAprobadoPor(int usuarioId)
    {
        this.UsuarioId_Aprobador_Combustible = usuarioId;
    }

    public void CancelarComision()
    {
        this.Estado = ComisionEstados.Cancelada;

        if(this.Vehiculo is not null)
        {
            this.Vehiculo.Estado = EstadosTipos.Disponible;
        }

        
    }
    public void CompletarComision()
    {
        this.Estado = ComisionEstados.Completada;
        if(this.Vehiculo is not null)
        {
            this.Vehiculo.Estado = EstadosTipos.Disponible;
        }
        if(this.ComisionUsuarios is not null)
        {
            foreach(var comisionUsuario in this.ComisionUsuarios)
            {
                comisionUsuario.Estado = EstadosTipos.Completada;
            }
        }
    }



}