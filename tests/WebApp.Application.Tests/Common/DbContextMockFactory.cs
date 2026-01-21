using WebApp.Persistence;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Application.Tests.Common;
public static class DbContextMockFactory
{
    public static WebAppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<WebAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        return new WebAppDbContext(options);
    }
}