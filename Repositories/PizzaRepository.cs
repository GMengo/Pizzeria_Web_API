using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using Microsoft.Data.SqlClient;
using pizzeria_web_api.Models;

namespace pizzeria_web_api.Repositories
{
    public class PizzaRepository
    {

        private const string connectionString = "Data Source=localhost;Initial Catalog=PizzeriaDB;Integrated Security=True;TrustServerCertificate=True";

        public void ReadPizza(SqlDataReader r, Dictionary<int,Pizza> pizze)
        {
            int id = r.GetInt32(r.GetOrdinal("Id"));

            if (pizze.TryGetValue(id, out Pizza pizza) == false)
            {
                string nome = r.GetString(r.GetOrdinal("nome"));
                string descrizione = r.GetString(r.GetOrdinal("descrizione"));
                decimal prezzo = (decimal)r.GetDouble(r.GetOrdinal("prezzo"));
                pizza = new Pizza(id, nome, descrizione, prezzo);
                pizze.Add(id, pizza);

            }
            if (r.IsDBNull(r.GetOrdinal("Id_Categoria")) == false)
            {
                int categoriaId = r.GetInt32(r.GetOrdinal("Id_Categoria"));
                string nomeCategoria = r.GetString(r.GetOrdinal("nome_Categoria"));
                Categoria c = new();
                c.Id = r.GetInt32(r.GetOrdinal("Id_Categoria"));
                c.Nome = r.GetString(r.GetOrdinal("nome_Categoria"));
                pizza.CategoriaId = c.Id;
                pizza.Categoria = c;
            }

            if (r.IsDBNull(r.GetOrdinal("Id_Ingrediente")) == false)
            {
                var ingredienteId = r.GetInt32(r.GetOrdinal("Id_Ingrediente"));
                var ingredienteNome = r.GetString(r.GetOrdinal("nome_Ingrediente"));
                Ingrediente i = new Ingrediente(ingredienteId, ingredienteNome);
                pizza.Ingrediente.Add(i);
            }
        }

        public async Task<List<Pizza>> GetAllPizza()
        {
            Dictionary<int, Pizza> Pizze = new Dictionary<int, Pizza>();

            string query = @"SELECT p.*, C.Id as Id_Categoria, C.nome as nome_Categoria, I.Id as Id_Ingrediente, I.nome as nome_Ingrediente
                             FROM pizza p
                             LEFT JOIN Categoria C on p.CategoriaId = C.Id
                             LEFT JOIN PizzaIngrediente PI on PI.pizzaId = P.Id
                             LEFT JOIN Ingrediente I on PI.ingredienteId = I.Id
                            ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ReadPizza(reader, Pizze);
                        }
                    }
                }
                return Pizze.Values.ToList();
            }
        }
        public async Task<List<Pizza>> GetPizzaByName(string nome)
        {
            Dictionary<int, Pizza> Pizze = new Dictionary<int, Pizza>();

            string query = @"SELECT p.*, C.Id as Id_Categoria, C.nome as nome_Categoria, I.Id as Id_Ingrediente, I.nome as nome_Ingrediente
                             FROM pizza p
                             LEFT JOIN Categoria C on p.CategoriaId = C.Id
                             LEFT JOIN PizzaIngrediente PI on PI.pizzaId = P.Id
                             LEFT JOIN Ingrediente I on PI.ingredienteId = I.Id
                             WHERE p.nome like @nome
                            ";
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
                            ReadPizza(reader, Pizze);

                        }
                    }
                }
                return Pizze.Values.ToList();
            }
        }

        public async Task<Pizza> GetPizzaByIdAsync(int id)
        {
            Dictionary<int, Pizza> Pizze = new Dictionary<int, Pizza>();

            string query = @"SELECT p.*, C.Id as Id_Categoria, C.nome as nome_Categoria, I.Id as Id_Ingrediente, I.nome as nome_Ingrediente
                             FROM pizza p
                             LEFT JOIN Categoria C on p.CategoriaId = C.Id
                             LEFT JOIN PizzaIngrediente PI on PI.pizzaId = P.Id
                             LEFT JOIN Ingrediente I on PI.ingredienteId = I.Id
                             WHERE p.id = @id
                            ";

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
                            ReadPizza(reader,Pizze);
                            
                        }

                    }
                }

            }
            return Pizze.Values.FirstOrDefault();
        }

        public async Task<(int, Pizza)> CreatePizza(Pizza p)
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


                    return (await command.ExecuteNonQueryAsync(), p); // ritorna una tupla, contenente il numero di righe "modificate" e l' oggetto creato
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
