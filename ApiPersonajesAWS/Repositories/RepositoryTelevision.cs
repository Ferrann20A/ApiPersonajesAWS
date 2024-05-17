using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryTelevision
    {
        private TelevisionContext context;

        public RepositoryTelevision(TelevisionContext context)
        {
            this.context = context;
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        private async Task<int> GetMaxIdPersonajeAsync()
        {
            return await this.context.Personajes.MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task InsertPersonajeAsync(string nombre, string imagen)
        {
            Personaje per = new Personaje
            {
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };
            await this.context.Personajes.AddAsync(per);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePersonajeAsync(int idpersonaje, string nombre, string imagen)
        {
            string sql = "CALL UpdatePersonaje(@idpersonaje, @nombre, @imagen)";
            MySqlParameter pamIdPer = new MySqlParameter("@idpersonaje", idpersonaje);
            MySqlParameter pamNombre = new MySqlParameter("@nombre", nombre);
            MySqlParameter pamImagen = new MySqlParameter("@imagen", imagen);
            this.context.Database.ExecuteSqlRaw(sql, pamIdPer, pamNombre, pamImagen);
        }
    }
}
