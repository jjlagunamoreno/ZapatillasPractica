using Microsoft.EntityFrameworkCore;
using ZapatillasPractica.Data;
using ZapatillasPractica.Models;

namespace ZapatillasPractica.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;
        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }



        //OBTENEMOS TODAS LAS ZAPATILLAS
        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }
        //BUSCAMOS UNA ZAPATILLA POR SU ID
        public async Task<Zapatilla> FindZapatillaAsync(int id)
        {
            return await this.context.Zapatillas
                .FirstOrDefaultAsync(z => z.IdZapatilla == id);
        }
        //OBTENEMOS TODAS LAS IMÁGENES DE UNA ZAPATILLA
        public async Task<List<ImagenZapatilla>> GetImagenesZapatillaAsync(int idZapatilla)
        {
            return await this.context.ImagenesZapatilla
                .Where(x => x.IdZapatilla == idZapatilla)
                .ToListAsync();
        }
    }
}
