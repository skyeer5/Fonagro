using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain;
using WebApp.Persistence;
using WebApp.Persistence.Models;

namespace WebApp.Web.Extensions;

public static class DataSeed
{
    public static async Task SeedDataAuthentication(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<WebAppDbContext>();
            await context.Database.MigrateAsync();
            
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            if (!userManager.Users.Any())
            {
                var userAdmin = new AppUser
                {
                    Nombre_Completo = "Encargado de Sistemas",
                    UserName = RolesTipos.Administrador,
                    Email = "soporte.ti@fonagro.gob.gt",
                    NIT = "117752649"
                };
                var result = await userManager.CreateAsync(userAdmin, "F0n@gr02026");
                if (!result.Succeeded)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
                await userManager.AddToRoleAsync(userAdmin, RolesTipos.Administrador);
            }

            if(!context.Gasolinas.Any())
            {
                var gasolinas = new List<Gasolina>
                {
                    new Gasolina { Nombre = GasolinaTipos.Super},
                    new Gasolina { Nombre = GasolinaTipos.Regular},
                    new Gasolina { Nombre = GasolinaTipos.Disel}
                };
                await context.Gasolinas.AddRangeAsync(gasolinas);
            }

            if(!context.Viaticos.Any())
            {
                var viaticos = new List<Viatico>
                {
                    new Viatico { Nombre = ViaticosTipos.Desayuno, Monto = 63, Vigente = true},
                    new Viatico { Nombre = ViaticosTipos.Almuerzo, Monto = 84, Vigente = true},
                    new Viatico { Nombre = ViaticosTipos.Cena, Monto = 63, Vigente = true},
                    new Viatico { Nombre = ViaticosTipos.Hospedaje, Monto = 210, Vigente = true}
                };
                await context.Viaticos.AddRangeAsync(viaticos);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<WebAppDbContext>();
            logger.LogError(e.Message);
        }
    }
}