using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WebAppDbContext>(opt =>
        {
            
            // opt.LogTo(Console.WriteLine, new[]
            //     {DbLoggerCategory.Database.Command.Name},
            //     LogLevel.Information
            //     ).EnableSensitiveDataLogging();
            opt.UseMySql(configuration.GetConnectionString("MySql"), new MySqlServerVersion(new Version(11, 8, 6)));
        });
        return services;
    }
}