using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZapatillasPractica.Models;
using ZapatillasPractica.Repositories;

namespace ZapatillasPractica.Controllers
{
    public class ZapatillasController : Controller
    {
        private readonly ILogger<ZapatillasController> _logger;
        private readonly RepositoryZapatillas _repo;

        public ZapatillasController(ILogger<ZapatillasController> logger, RepositoryZapatillas repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // CARGA LISTADO DE ZAPATILLAS
        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapatillas = await _repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        // MUESTRA DETALLES Y IMAGEN EN POSICIÓN DADA
        public async Task<IActionResult> Details(int? posicion, int zapatilla)
        {
            if (posicion == null) { posicion = 1; }

            (int totalRegistros, ImagenZapatilla? imagen) =
                await _repo.GetImagenesZapatillaAsync(posicion.Value, zapatilla);

            Zapatilla zap = await _repo.FindZapatillaAsync(zapatilla);

            ViewData["ZAPATILLA"] = zap;
            ViewData["REGISTROS"] = totalRegistros;
            ViewData["POSICION"] = posicion;

            return View(imagen);
        }

        // DEVUELVE SOLO LA IMAGEN VÍA AJAX
        public async Task<IActionResult> GetImagen(int posicion, int zapatilla)
        {
            (int totalRegistros, ImagenZapatilla? imagen) =
                await _repo.GetImagenesZapatillaAsync(posicion, zapatilla);

            // SI NO HAY IMAGEN, VERIFICAMOS SI ES PORQUE LA POSICIÓN SE SALIÓ DE RANGO
            if (imagen == null && totalRegistros > 0)
            {
                // SI POSICIÓN ESTÁ POR DEBAJO DE 1, LA AJUSTAMOS A 1
                if (posicion < 1) posicion = 1;
                // SI EXCEDE totalRegistros, LA AJUSTAMOS AL LÍMITE
                if (posicion > totalRegistros) posicion = totalRegistros;

                // REEJECUTAMOS LA PETICIÓN PARA OBTENER LA IMAGEN
                (totalRegistros, imagen) = await _repo.GetImagenesZapatillaAsync(posicion, zapatilla);
            }

            // SI SIGUE SIENDO NULL, SIGNIFICA QUE NO HAY IMÁGENES
            if (imagen == null)
            {
                return Content("No existen imágenes para esta zapatilla");
            }

            ViewData["ZAPATILLA_ID"] = zapatilla;
            ViewData["REGISTROS"] = totalRegistros;
            ViewData["POSICION"] = posicion;

            return PartialView("_ImagenPartial", imagen);
        }
    }
}
