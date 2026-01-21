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
                    Email = "soporte.ti@fonagro.gob.gt"
                };
                var result = await userManager.CreateAsync(userAdmin, "F0n@gr02026");
                if (!result.Succeeded)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
                await userManager.AddToRoleAsync(userAdmin, RolesTipos.Administrador);
            }
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<WebAppDbContext>();
            logger.LogError(e.Message);
        }
    }
}