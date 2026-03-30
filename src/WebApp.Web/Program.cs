using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Web.Application.Interfaces;
using WebApp.Application;
using WebApp.Application.Interfaces;
using WebApp.Infrastructure.Identity;
using WebApp.Infrastructure.Jobs;
using WebApp.Infrastructure.Policies;
using WebApp.Infrastructure.Repositories;
using WebApp.Infrastructure.Services;
using WebApp.Persistence;
using WebApp.Persistence.Models;
using WebApp.Web.Extensions;
using WebApp.Web.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGasolinaService, GasolinaService>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IGasolinaPrecioService, GasolinaPrecioService>();
builder.Services.AddScoped<IComisionRepository, ComisionRepository>();
builder.Services.AddScoped<IComisionUsuarioPolicy, ComisionUsuarioPolicy>();
builder.Services.AddScoped<IPartesService, PartesService>();
builder.Services.AddScoped<IAccesoriosService, AccesoriosService>();
builder.Services.AddScoped<IViaticosService, ViaticosService>();
builder.Services.AddScoped<IComisionService, ComisionService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IComisionUsuarioService, ComisionUsuarioService>();
builder.Services.AddScoped<IComisionUsuarioRepository, ComisionUsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGasolinaPrecioRepository, GasolinaPrecioRepository>();
builder.Services.AddScoped<IBackgroundJob, ComisionEstadoJob>();
builder.Services.AddHostedService<SchedulerService>();


builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<WebAppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/Login";

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

await app.SeedDataAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
