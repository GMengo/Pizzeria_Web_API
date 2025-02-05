using System.Runtime.ConstrainedExecution;
using Microsoft.Data.SqlClient;

namespace pizzeria_web_api
{
    public class PizzaRepository
    {

        private const string connectionString = "Data Source=localhost;Initial Catalog=PizzeriaDB;Integrated Security=True;Trust Server Certificate=True";

        public Pizza ReadPizza(SqlDataReader r)
        {
            Pizza pizza = new Pizza();
            pizza.Id = r.GetInt32(r.GetOrdinal("Id"));
            pizza.Nome = r.GetString(r.GetOrdinal("nome"));
            pizza.Descrizione = r.GetString(r.GetOrdinal("descrizione"));
            pizza.Prezzo = (decimal)r.GetDouble(r.GetOrdinal("prezzo"));

            if (!r.IsDBNull(r.GetOrdinal("categoriaId")))
            {
                Categoria c = new();
                c.Id = r.GetInt32(r.GetOrdinal("Id"));
                c.Nome = r.GetString(r.GetOrdinal("nome"));
                pizza.CategoriaId = c.Id;
                pizza.Categoria = c;
            }

            return pizza;
        }

        public async Task<List<Pizza>> GetAllPizza()
        {
            List<Pizza> ListaPizze = new List<Pizza>();

            string query = "SELECT p.*, C.Id, C.nome FROM pizza p LEFT JOIN Categoria C on p.CategoriaId = C.Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Pizza p = ReadPizza(reader);
                            ListaPizze.Add(p);
                        }
                    }
                }
                return ListaPizze;
            }
        }
        public async Task<List<Pizza>> GetPizzaByName(string nome)
        {
            List<Pizza> ListaPizze = new List<Pizza>();

            string query = "SELECT * FROM pizza WHERE nome like @nome";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nome", $"%{nome}%");
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Pizza p = ReadPizza(reader);
                            ListaPizze.Add(p);
                            //ListaPizze.Add(ReadPizza(reader));  scritto in una sola riga 
                        }
                    }
                }
                return ListaPizze;
            }
        }

        public async Task<Pizza> GetPizzaByIdAsync(int id)
        {
            string query = "SELECT * FROM pizza where id = @id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Pizza p = ReadPizza(reader);
                            return p;
                        }
                        
                    }
                }
                
            }
            return null;
        }

        public async Task<int> CreatePizzaParams(string nome, string descrizione, decimal prezzo)
        {
            string query = "INSERT INTO Pizza(nome, descrizione, prezzo) VALUES (@nome, @descrizione, @prezzo)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@nome", nome);
                    command.Parameters.AddWithValue("@descrizione", descrizione);
                    command.Parameters.AddWithValue("@prezzo", prezzo);
                    
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<(int, Pizza)> CreatePizzaBody(Pizza p)
        {
            string query = "INSERT INTO Pizza(nome, descrizione, prezzo) VALUES (@nome, @descrizione, @prezzo)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@nome", p.Nome);
                    command.Parameters.AddWithValue("@descrizione", p.Descrizione);
                    command.Parameters.AddWithValue("@prezzo", p.Prezzo);


                    return (await command.ExecuteNonQueryAsync(),p); // ritorna una tupla, contenente il numero di righe "modificate" e l' oggetto creato
                }
            }
        }

        public async Task<int> UpdatePizza(int id, Pizza p)
        {
            string query = "UPDATE Pizza SET nome = @nome, descrizione = @descrizione, prezzo = @prezzo WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@nome", p.Nome);
                    command.Parameters.AddWithValue("@descrizione", p.Descrizione);
                    command.Parameters.AddWithValue("@prezzo", p.Prezzo);
                    return await command.ExecuteNonQueryAsync();
                }
            }

        }

        public async Task<int> DeletePizza(int id)
        {
            string query = "DELETE FROM pizza where id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
