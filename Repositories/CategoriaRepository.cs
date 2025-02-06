using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using pizzeria_web_api.Models;

namespace pizzeria_web_api.Repositories
{
    public class CategoriaRepository
    {
        private const string connectionString = "Data Source=localhost;Initial Catalog=PizzeriaDB;Integrated Security=True;TrustServerCertificate=True";
        public CategoriaRepository() { }

        public Categoria ReadCategoria(SqlDataReader reader)
        {
            Categoria c = new Categoria();
            c.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            c.Nome = reader.GetString(reader.GetOrdinal("nome"));
            return c;
        }

        public async Task<List<Categoria>> GetCategorie()
        {
            List<Categoria> listaCategorie = new List<Categoria>();
            string query = "SELECT * FROM Categorie";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Categoria c = ReadCategoria(reader);
                            listaCategorie.Add(c);
                        }
                    }
                }
                return listaCategorie;
            }

        }

    }
}
