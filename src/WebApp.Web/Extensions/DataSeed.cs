using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain;
using WebApp.Domain.Usuarios;
using WebApp.Persistence;
using WebApp.Persistence.Models;
using WebApp.Domain.Gasolinas;
using WebApp.Domain.Viaticos;
using WebApp.Application.Interfaces;
using WebApp.Domain.Unidades;
using WebApp.Domain.Puestos;


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
        var locacionesService = service.GetRequiredService<ILocacionesService>();

        try
        {
            var context = service.GetRequiredService<WebAppDbContext>();
            await context.Database.MigrateAsync();
            
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            if (!userManager.Users.Any())
            {
                var userAdmin = new AppUser
                {
                    Nombres = "Encargado de Sistemas",
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

            if(!context.Departamentos.Any())
            {
                var deptamentos = await locacionesService.GetDepartamentosDataSeedAsync();
                await context.Departamentos.AddRangeAsync(deptamentos!);
            }
            if(!context.Unidades.Any())
            {
                var unidades = new List<Unidad>();
                //GERENCIA
                var gerencia = new Unidad { Nombre = "GERENCIA" };
                gerencia.Puestos = new List<Puesto>
                {
                    new Puesto { Nombre = "GERENTE GENERAL"},
                    new Puesto { Nombre = "ASISTENTE DE GERENCIA"},
                    new Puesto { Nombre = "ASESOR DE GERENCIA"},
                };
                unidades.Add(gerencia);

                //UNIDAD ADMINISTRATIVA
                var ua = new Unidad { Nombre = "UNIDAD ADMINISTRATIVA" };
                ua.Puestos = new List<Puesto>
                {
                    new Puesto { Nombre = "COORDINADOR DE LA UNIDAD ADMINISTRATIVA"},
                    new Puesto { Nombre = "SUBCOORDINADOR DE LA UNIDAD ADMINISTRATIVA"},
                    new Puesto { Nombre = "ASESOR DE LA UNIDAD ADMINISTRATIVA"},
                    new Puesto { Nombre = "ASISTENTE DE LA UNIDAD ADMINISTRATIVA"},
                    // Encargados
                    new Puesto { Nombre = "ENCARGADO DE ADMINISTRACIÓN Y PERSONAL" },
                    new Puesto { Nombre = "ENCARGADO DE SERVICIOS GENERALES" },
                    new Puesto { Nombre = "ENCARGADO DE SISTEMAS" },
                    new Puesto { Nombre = "ENCARGADO DE ARCHIVO" },
                    new Puesto { Nombre = "ENCARGADO DE COMUNICACIÓN E INFORMACIÓN PÚBLICA" },

                    // Auxiliares
                    new Puesto { Nombre = "AUXILIAR DE SERVICIOS GENERALES" },
                    new Puesto { Nombre = "AUXILIAR DE SISTEMAS" },
                    new Puesto { Nombre = "AUXILIAR DE ARCHIVO" },
                    new Puesto { Nombre = "AUXILIAR DE COMUNICACIÓN E INFORMACIÓN PÚBLICA" },
                    new Puesto { Nombre = "PILOTO MENSAJERO" }
                };
                unidades.Add(ua);
                
                //UNIDAD DE ASUNTOS JURÍDICOS
                var uaj = new Unidad { Nombre = "UNIDAD DE ASUNTOS JURÍDICOS" };
                uaj.Puestos = new List<Puesto>
                {
                    new Puesto { Nombre = "COORDINADOR DE ASESORÍA JURÍDICA"},
                    new Puesto { Nombre = "SUBCOORDINADOR DE ASESORÍA JURÍDICA"},
                    new Puesto { Nombre = "ASESOR JURÍDICO"},
                    new Puesto { Nombre = "ASISTENTE DE LA UNIDAD DE ASUNTOS JURÍDICOS"},
                };
                unidades.Add(uaj);

                //UNIDAD TÉCNICA DE SEGUIMIENTO Y EVALUACIÓN
                var utse = new Unidad { Nombre = "UNIDAD TÉCNICA DE SEGUIMIENTO Y EVALUACIÓN" };
                utse.Puestos = new List<Puesto>
                {
                    new Puesto { Nombre = "COORDINADOR DE LA UNIDAD TÉCNICA DE SEGUIMIENTO Y EVALUACIÓN"},
                    new Puesto { Nombre = "SUBCOORDINADOR DE LA UNIDAD TÉCNICA DE SEGUIMIENTO Y EVALUACIÓN"},
                    new Puesto { Nombre = "ASESOR DE EVALUACIÓN, SEGUIMIENTO Y LIQUIDACIÓN DE PROYECTOS"},
                    new Puesto { Nombre = "ASISTENTE DE LA UNIDAD TÉCNICA DE SEGUIMIENTO Y EVALUACIÓN"},
                    //ENCARGADOS
                    new Puesto { Nombre = "ENCARGADO DE PLANIFICACIÓN" },
                    new Puesto { Nombre = "ENCARGADO DE RECUPERACIÓN DE CARTERA" },
                };
                unidades.Add(utse);

                //UNIDAD DE AUDITORÍA INTERNA
                var udai = new Unidad { Nombre = "UNIDAD DE AUDITORÍA INTERNA" };
                udai.Puestos = new List<Puesto>
                {
                    new Puesto { Nombre = "COORDINADOR DE AUDITORÍA INTERNA"},
                    new Puesto { Nombre = "SUBCOORDINADOR DE AUDITORÍA INTERNA"},
                    new Puesto { Nombre = "ASESOR DE AUDITORÍA INTERNA"},
                    new Puesto { Nombre = "ASISTENTE DE LA UNIDAD DE AUDITORÍA INTERNA"},
                    new Puesto { Nombre = "AUDITOR"},
                };
                unidades.Add(udai);
                await context.Unidades.AddRangeAsync(unidades!);
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