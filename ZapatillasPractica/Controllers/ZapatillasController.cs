using Microsoft.AspNetCore.Mvc;
using ZapatillasPractica.Models;
using ZapatillasPractica.Repositories;

public class ZapatillasController : Controller
{
    private RepositoryZapatillas repo;

    public ZapatillasController(RepositoryZapatillas repo)
    {
        this.repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        // Muestra lista de zapatillas
        List<Zapatilla> zaps = await this.repo.GetZapatillasAsync();
        return View(zaps);
    }

    // Muestra un detalle de la zapatilla con la vista parcial de imágenes
    public async Task<IActionResult> Details(int id)
    {
        Zapatilla zap = await this.repo.FindZapatillaAsync(id);
        return View(zap);
        // La vista "Details.cshtml" tendrá un contenedor <div> donde luego con Ajax cargarás la partial
    }

}
