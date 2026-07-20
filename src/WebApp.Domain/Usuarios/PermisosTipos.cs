namespace WebApp.Domain.Usuarios;

public static class PermisosTipos
{
    // Claim Types
    public const string Permiso = nameof(Permiso);

    //Claim Values-----------------------------------------
    //Comision
    public const string CrearComision = nameof(CrearComision);
    public const string AprobarComision = nameof(AprobarComision);
    public const string ConsultarComision = nameof(ConsultarComision);
    public const string EliminarComision = nameof(EliminarComision);
    public const string ModificarComision = nameof(ModificarComision);
    //Comision Destinos
    public const string CrearComision_Destinos = nameof(CrearComision_Destinos);
    public const string ModificarComision_Destinos = nameof(ModificarComision_Destinos);
    public const string EliminarComision_Destinos = nameof(EliminarComision_Destinos);
    //Gasolina
    public const string CrearGasolina = nameof(CrearGasolina);
    public const string ModificarGasolina = nameof(ModificarGasolina);
    public const string EliminarGasolina = nameof(EliminarGasolina);
    //Gasolina Precio
    public const string CrearPrecioGasolina = nameof(CrearPrecioGasolina);
    //Viatico
    public const string CrearViatico = nameof(CrearViatico);
    public const string ConsultarViatico = nameof(ConsultarViatico);
    public const string EliminarViatico = nameof(EliminarViatico);


}