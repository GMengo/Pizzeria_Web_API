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
    }
}
