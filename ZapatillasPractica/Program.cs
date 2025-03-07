using Microsoft.EntityFrameworkCore;
using ZapatillasPractica.Data;
using ZapatillasPractica.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//REPOSITORY
builder.Services.AddTransient<RepositoryZapatillas>();
//BBDD "Zapatillas"
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//DATA/CONTEXT
builder.Services.AddDbContext<ZapatillasContext>(options => options.UseSqlServer(connectionString));

//VIEWS
builder.Services.AddControllersWithViews();

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
