using System.Data;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZapatillasPractica.Data;
using ZapatillasPractica.Models;

#region procedure
//ALTER PROCEDURE SP_IMAGENES_ZAPATILLA
//(@posicion INT, @zapatilla INT, @registros INT OUT)
//AS
//BEGIN
//    SELECT @registros = COUNT(*) 
//    FROM IMAGENESZAPASPRACTICA 
//    WHERE IDPRODUCTO = @zapatilla;
//WITH cte AS
//    (
//        SELECT ROW_NUMBER() OVER (ORDER BY IDIMAGEN) AS POSICION,
//           IDIMAGEN, IDPRODUCTO, IMAGEN
//        FROM IMAGENESZAPASPRACTICA
//        WHERE IDPRODUCTO = @zapatilla
//    )
//    SELECT IDIMAGEN, IDPRODUCTO, IMAGEN
//    FROM cte
//    WHERE POSICION = @posicion;
//END;
//GO
#endregion

namespace ZapatillasPractica.Repositories
{
    public class RepositoryZapatillas
    {
        private readonly ZapatillasContext _context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            _context = context;
        }

        // OBTIENE TODAS LAS ZAPATILLAS
        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await _context.Zapatillas.ToListAsync();
        }

        // BUSCA ZAPATILLA POR SU ID
        public async Task<Zapatilla> FindZapatillaAsync(int id)
        {
            return await _context.Zapatillas
                .FirstOrDefaultAsync(z => z.IdZapatilla == id);
        }

        // EJECUTA SP PARA PAGINAR IMÁGENES (TUPLA: TOTAL, IMAGEN)
        public async Task<(int registros, ImagenZapatilla? imagen)>
            GetImagenesZapatillaAsync(int posicion, int zapatilla)
        {
            var paramOut = new SqlParameter("@registros", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var paramPos = new SqlParameter("@posicion", posicion);
            var paramZapatilla = new SqlParameter("@zapatilla", zapatilla);

            // PRIMERO, SOLO EJECUTAMOS PARA RELLENAR @registros
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_IMAGENES_ZAPATILLA @posicion, @zapatilla, @registros OUT",
                paramPos, paramZapatilla, paramOut
            );

            int total = (paramOut.Value == DBNull.Value)
                ? 0
                : (int)paramOut.Value;

            // DESPUÉS, OBTENEMOS LA IMAGEN CON LA MISMA LLAMADA
            var lista = await _context.Imagenes
                .FromSqlRaw(
                    "EXEC SP_IMAGENES_ZAPATILLA @posicion, @zapatilla, @registros OUT",
                    paramPos, paramZapatilla, paramOut
                )
                .AsNoTracking()
                .ToListAsync();

            ImagenZapatilla? imagen = lista.FirstOrDefault();

            return (total, imagen);
        }
    }
}
