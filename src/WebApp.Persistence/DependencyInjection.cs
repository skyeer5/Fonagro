using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            
            opt.LogTo(Console.WriteLine, new[]
                {DbLoggerCategory.Database.Command.Name},
                LogLevel.Information
                ).EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            opt.UseSqlServer(configuration.GetConnectionString("SqlServer"))
                .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        });
        return services;
    }
}