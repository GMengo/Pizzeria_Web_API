using Microsoft.Data.SqlClient;
using pizzeria_web_api.Models;

namespace pizzeria_web_api.Repositories
{
    public class IngredienteRepository
    {
        private const string connectionString = "Data Source=localhost;Initial Catalog=PizzeriaDB;Integrated Security=True;TrustServerCertificate=True";

        private Ingrediente ReadIngrediente(SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("id"));
            var nome = reader.GetString(reader.GetOrdinal("nome"));
            var Ingrediente = new Ingrediente(id, nome);
            return Ingrediente;
        }

        public async Task<List<Ingrediente>> GetAllIngredientisAsync()
        {
            string query = "SELECT * FROM ingrediente";
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            List<Ingrediente> ingredienti = new List<Ingrediente>();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ingredienti.Add(ReadIngrediente(reader));
                    }
                }
            }
            return ingredienti;
        }

        public async Task<Ingrediente> GetIngredienteByIdAsync(int id)
        {
            string query = "SELECT * FROM ingrediente where id = @id";
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Ingrediente ingrediente = ReadIngrediente(reader);
                    return ingrediente;
                }

            }
            return null;
        }

        public async Task<Ingrediente> InsertIngredient(Ingrediente ingrediente)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = $"INSERT INTO ingrediente (nome) VALUES (@nome);" +
                        $"SELECT SCOPE_IDENTITY();";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@nome", ingrediente.Nome));

                ingrediente.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                return ingrediente;
            }
        }

        public async Task<int> UpdateIngredienteAsync(int id, Ingrediente ingrediente)
        {
            string query = "UPDATE Ingrediente SET nome = @nome WHERE Id = @id";
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nome", ingrediente.Nome);

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteIngredienteAsync(int id)
        {
            string query = "DELETE FROM ingrediente WHERE Id = @id";
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(query,conn);

            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> ClearPizzaIngredienteAsync(int ingredienteId)
        {
            string query = "DELETE FROM pizzaingrediente WHERE ingredienteId = @ingredienteId";
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ingredienteId", ingredienteId);

            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
