using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Application;
using WebApp.Application.Interfaces;
using WebApp.Infrastructure.Identity;
using WebApp.Infrastructure.Policies;
using WebApp.Infrastructure.Repositories;
using WebApp.Infrastructure.Services;
using WebApp.Persistence;
using WebApp.Persistence.Models;
using WebApp.Web.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
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

builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<WebAppDbContext>()
    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityCore<AppUser>( opt=>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<WebAppDbContext>();

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

app.UseAuthorization();

await app.SeedDataAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
