using Microsoft.AspNetCore.Identity;
using WebApp.Application;
using WebApp.Persistence;
using WebApp.Persistence.Models;
using WebApp.Web.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

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
