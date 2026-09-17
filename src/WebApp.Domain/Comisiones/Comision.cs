namespace WebApp.Domain.Comisiones;
using WebApp.Domain.ComisionDestinos;
using WebApp.Domain.Vehiculos;
using WebApp.Domain.Viaticos;
using WebApp.Domain.Nombramientos;
public class Comision : AuditableEntity
{
    public int ComisionId { get; set; }
    public decimal Precio_Gasolina_Usado { get; set; }
    public decimal Presupuesto_Combustible_Estimado { get; set; }
    public int? UsuarioId_Aprobador_Combustible { get; set; }
    public decimal? Presupuesto_Combustible_Aprobado { get; set; }
    public decimal Kilometraje_Inicial { get; set; }
    public decimal Kilometraje_Final { get; set; }
    public DateTime Fecha {get; set; }
    public ComisionEstados Estado { get; set; }
    public int? NombramientoId_Respon_Vehiculo { get; set; }
    public Nombramiento? Nombramiento_Respon_Vehiculo { get; set; }
    public TimeOnly Hora_Salida { get; set; }
    public TimeOnly Hora_Regreso { get; set; }
    public int UsuarioId_Creador_Comision { get; set; }
    public int? VehiculoId { get; set; } 
    public Vehiculo? Vehiculo { get; set; }
    public ICollection<ComisionDestino>? ComisionDestinos { get; set; }
    public ICollection<Nombramiento>? Nombramientos { get; set; } 

    public static Comision Crear(TimeOnly hora_salida, TimeOnly hora_regreso)
    {
        return new Comision
        {
            Hora_Salida = hora_salida,
            Hora_Regreso = hora_regreso,
            Fecha = DateTime.Now,
            Estado = ComisionEstados.Creada
        };
    }

    public void AgregarVehiculo(int vehiculoId, decimal gasolinaPrecio)
    {
        this.Precio_Gasolina_Usado = gasolinaPrecio;
        this.VehiculoId = vehiculoId;
    }
    public void AgregarCreadoPor(int usuarioId)
    {
        this.UsuarioId_Creador_Comision = usuarioId;
        this.Fecha = DateTime.Now;
        this.CreatedBy = usuarioId;
        this.CreatedDate = DateTime.Now;
    }
    public void AgregarUsuarios(List<Nombramiento> usuariosNombrados, List<Viatico> viaticos)
    {
        usuariosNombrados = usuariosNombrados.Distinct().ToList();
        this.Nombramientos = usuariosNombrados;


        var usuarioPiloto = usuariosNombrados.First();
        this.Nombramiento_Respon_Vehiculo = usuarioPiloto;

        foreach(var usuario in usuariosNombrados)
        {
            usuario.AsignarViaticos(viaticos, Hora_Salida, Hora_Regreso);            
        }
        
    }
    public void AgregarDestinos(List<ComisionDestino> comisionDestinos, int userId)
    {
        this.ComisionDestinos ??= new List<ComisionDestino>();

        foreach(var destino in comisionDestinos)
        {
            destino.CreatedBy = userId;
            destino.CreatedDate = DateTime.Now;
            this.ComisionDestinos.Add(destino);
        }
        var Galon_Estimado = comisionDestinos.Sum( x=>x.Galones);
        this.Presupuesto_Combustible_Estimado = decimal.Multiply(Precio_Gasolina_Usado, Galon_Estimado);
        this.Estado = ComisionEstados.DestinosDefinidos;

    }
    public void AgregarDestinosSinVehiculo(List<ComisionDestino> comisionDestinos, int userId)
    {
        this.ComisionDestinos ??= new List<ComisionDestino>();

        foreach(var destino in comisionDestinos)
        {
            destino.CreatedBy = userId;
            destino.CreatedDate = DateTime.Now;
            this.ComisionDestinos.Add(destino);
        }
        this.Estado = ComisionEstados.Programada;

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
            this.Vehiculo.ModificarEstadoDisponible();
        }        
    }
    public void CompletarComision()
    {
        this.Estado = ComisionEstados.Completada;
        if(this.Vehiculo is not null)
        {
            this.Vehiculo.ModificarEstadoDisponible();
        }
        if(this.Nombramientos is not null)
        {
            foreach(var comisionUsuario in this.Nombramientos)
            {
                comisionUsuario.Estado = NombramientoEstados.Completada;
            }
        }
    }



}