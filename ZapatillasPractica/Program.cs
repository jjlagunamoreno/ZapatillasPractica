using Microsoft.EntityFrameworkCore;
using ZapatillasPractica.Data;
using ZapatillasPractica.Repositories;

var builder = WebApplication.CreateBuilder(args);

// INYECTAMOS REPOSITORYZAPATILLAS
builder.Services.AddTransient<RepositoryZapatillas>();

// Cadena de conexión en appsettings.json => "DefaultConnection"
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ZapatillasContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Error handling genérico
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// RUTEAMOS A ZAPATILLAS INDEX DIRECTAMENTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Zapatillas}/{action=Index}/{id?}"
);

app.Run();
