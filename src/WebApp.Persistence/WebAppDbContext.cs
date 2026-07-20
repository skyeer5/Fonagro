using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain;
using WebApp.Persistence.Models;
using WebApp.Domain.Comisiones;
using WebApp.Domain.ComisionDestinos;
using WebApp.Domain.ComisionesViaticos;
using WebApp.Domain.Gasolinas;
using WebApp.Domain.GasolinaPrecios;
using WebApp.Domain.Vehiculos;

namespace WebApp.Persistence;

public class WebAppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Gasolina> Gasolinas { get; set; }
    public DbSet<GasolinaPrecio> GasolinaPrecios { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Viatico> Viaticos { get; set; }
    public DbSet<Comision> Comisiones { get; set; }
    public DbSet<ComisionDestino> ComisionDestinos { get; set; }
    public DbSet<ComisionUsuario> ComisionUsuarios { get; set; }
    public DbSet<ComisionViaticos> ComisionViaticos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Definicion de las tablas
        modelBuilder.Entity<Gasolina>().ToTable("gasolinas");
        modelBuilder.Entity<GasolinaPrecio>().ToTable("gasolinaPrecios");
        modelBuilder.Entity<Vehiculo>().ToTable("vehiculos");
        modelBuilder.Entity<Viatico>().ToTable("viaticos");
        modelBuilder.Entity<Comision>().ToTable("comisiones");
        modelBuilder.Entity<ComisionDestino>().ToTable("comisionDestinos");
        modelBuilder.Entity<ComisionUsuario>().ToTable("comisionUsuarios");
        modelBuilder.Entity<ComisionViaticos>().ToTable("comisionViaticos");

        // Definicion de las propiedades
        modelBuilder.Entity<GasolinaPrecio>()
            .Property(gp => gp.Precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Viatico>()
            .Property(gp => gp.Monto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Comision>()
            .Property(c => c.Precio_Galon_Usado)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Comision>()
            .Property(c => c.Galon_Estimado)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Comision>()
            .Property(c => c.Presupuesto_Combustible_Estimado)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Comision>()
            .Property(c => c.Presupuesto_Combustible_Aprobado)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ComisionDestino>()
            .Property(c => c.Kilometros)
            .HasPrecision(18, 2);
        modelBuilder.Entity<ComisionDestino>()
            .Property(c => c.Galones)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ComisionViaticos>()
            .Property(cv => cv.Monto_Unitario_Usado)
            .HasPrecision(18, 2);
        modelBuilder.Entity<ComisionViaticos>()
            .Property(cv => cv.Monto_Total)
            .HasPrecision(18, 2);

        // Definicion de las relaciones
        modelBuilder.Entity<Gasolina>()
            .HasMany(g => g.GasolinaPrecios)
            .WithOne(p => p.Gasolina)
            .HasForeignKey(p => p.GasolinaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Gasolina>()
            .HasMany(g => g.Vehiculos)
            .WithOne(p => p.Gasolina)
            .HasForeignKey(p => p.GasolinaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Comision>()
            .HasOne(c => c.Vehiculo)
            .WithMany(v => v.Comisiones)
            .HasForeignKey(c => c.VehiculoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comision>()
            .HasOne<AppUser>()
            .WithMany(v=>v.Comisiones)
            .HasForeignKey(c => c.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comision>()
            .HasMany(c => c.ComisionDestinos)
            .WithOne(d => d.Comision)
            .HasForeignKey(d => d.ComisionId)
            .IsRequired();

        modelBuilder.Entity<ComisionUsuario>()
        .HasKey(x=> new { x.ComisionId, x.UsuarioId });

        modelBuilder.Entity<ComisionUsuario>()
            .HasOne(cu => cu.Comision)
            .WithMany(c => c.ComisionUsuarios)
            .HasForeignKey(cu => cu.ComisionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComisionUsuario>()
            .HasOne<AppUser>()
            .WithMany(v => v.ComisionUsuarios)
            .HasForeignKey(cu => cu.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComisionViaticos>()
            .HasKey(x=>x.ComisionViaticosId);

        modelBuilder.Entity<ComisionViaticos>()
            .HasOne(cv => cv.ComisionUsuario)
            .WithMany(cu => cu.ComisionViaticosList)
            .HasForeignKey(cv => new { cv.ComisionId, cv.UsuarioId })
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ComisionViaticos>()
            .HasOne(cv => cv.Viatico)
            .WithMany(v => v.ComisionViaticos)
            .HasForeignKey(cv => cv.ViaticoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Carga de datos iniciales de seguridad
        CargarDataSeguridad(modelBuilder);
    }

    public void CargarDataSeguridad(ModelBuilder modelBuilder)
    {
        var admin = new IdentityRole<int> { Id = RolesTipos.AdministradorId, Name = RolesTipos.Administrador, NormalizedName = RolesTipos.Administrador.ToUpper() };
        var comisionista = new IdentityRole<int> { Id = RolesTipos.ComisionistaId, Name = RolesTipos.Comisionista, NormalizedName = RolesTipos.Comisionista.ToUpper() };
        var consultor = new IdentityRole<int> { Id = RolesTipos.ConsultorId, Name = RolesTipos.Consultor, NormalizedName = RolesTipos.Consultor.ToUpper() };
        var gestor = new IdentityRole<int> { Id = RolesTipos.GestorId, Name = RolesTipos.Gestor, NormalizedName = RolesTipos.Gestor.ToUpper() };
        var aprobador_gasolina = new IdentityRole<int> { Id = RolesTipos.Aprobador_GasolinaId, Name = RolesTipos.Aprobador_Gasolina, NormalizedName = RolesTipos.Aprobador_Gasolina.ToUpper() };
        var usuario = new IdentityRole<int> { Id = RolesTipos.UsuarioId, Name = RolesTipos.Usuario, NormalizedName = RolesTipos.Usuario.ToUpper() };
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            admin,
            comisionista,
            consultor,
            gestor,
            aprobador_gasolina,
            usuario
        );
        modelBuilder.Entity<IdentityRoleClaim<int>>().HasData(
            //Administrador 
            new IdentityRoleClaim<int>
            {
                Id = 1,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.CrearComision
            },
            new IdentityRoleClaim<int>
            {
                Id = 2,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.AprobarComision
            },
            new IdentityRoleClaim<int>
            {
                Id = 3,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.ConsultarComision
            },
            new IdentityRoleClaim<int>
            {
                Id = 4,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.EliminarComision
            },
            new IdentityRoleClaim<int>
            {
                Id = 5,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.ModificarComision
            },
            new IdentityRoleClaim<int>
            {
                Id = 6,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.CrearComision_Destinos
            },
            new IdentityRoleClaim<int>
            {
                Id = 7,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.ModificarComision_Destinos
            },
            new IdentityRoleClaim<int>
            {
                Id = 8,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.EliminarComision_Destinos
            },
            new IdentityRoleClaim<int>
            {
                Id = 9,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.CrearGasolina
            },
            new IdentityRoleClaim<int>
            {
                Id = 10,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.EliminarGasolina
            },
            new IdentityRoleClaim<int>
            {
                Id = 11,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.ModificarGasolina
            },
            new IdentityRoleClaim<int>
            {
                Id = 12,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.CrearPrecioGasolina
            },
            new IdentityRoleClaim<int>
            {
                Id = 13,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.CrearViatico
            },
            new IdentityRoleClaim<int>
            {
                Id = 14,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.EliminarViatico
            },
            new IdentityRoleClaim<int>
            {
                Id = 15,
                RoleId = RolesTipos.AdministradorId,
                ClaimType = PermisosTipos.Permiso,
                ClaimValue = PermisosTipos.ConsultarViatico
            }
        );
    }
}