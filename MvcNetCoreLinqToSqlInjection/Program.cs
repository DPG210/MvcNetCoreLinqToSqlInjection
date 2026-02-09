using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


Coche car = new Coche();
car.Marca = "PONTIAC";
car.Modelo = "FIREBIRD";
car.Imagen = "expedition33.jpg";
car.VelocidadMaxima = 280;
builder.Services.AddSingleton<ICoche, Coche>(x=>car);
//RESOLVEMOS EL SERVICIO Coche PARA LA INYECCION
//builder.Services.AddTransient<Coche>();
//builder.Services.AddSingleton<Coche>();
//builder.Services.AddSingleton<ICoche,Coche>();
//los repo suelen ir como AddTransient
//builder.Services.AddTransient<RepositoryDoctoresSQLServer>();
//builder.Services.AddTransient<RepositoryDoctoresOracle>();
builder.Services.AddTransient<IRepositoryDoctores, RepositoryDoctoresSQLServer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
