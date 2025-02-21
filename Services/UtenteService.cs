using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using pizzeria_web_api.Models;

namespace pizzeria_web_api.Services
{
    public class UtenteService
    {
        public const string connectionString = "Data Source=localhost;Initial Catalog=PizzeriaDB;Integrated Security=True;TrustServerCertificate=True";

        private readonly IPasswordHasher<UtenteModel> _passwordHasher;

        public UtenteService(IPasswordHasher<UtenteModel> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterAsync(UtenteModel utente)
        {
            string passwordHash = _passwordHasher.HashPassword(utente, utente.Password);

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Utente (Email, PasswordHash) VALUES (@Email, @PasswordHash)";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", utente.Email);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<Utente> AuthenticateAsync(string email, string password)
        {
            string query = "SELECT * FROM Utente WHERE Email = @Email";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue($"@Email", email);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("Id"));
                string passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                UtenteModel model = new UtenteModel() { Email = email, Password = password};
                if (_passwordHasher.VerifyHashedPassword(model, passwordHash, password) != PasswordVerificationResult.Success) 
                {
                    return null;
                }
                return new Utente() { Id = id, Email = email };
            }
            return null;
        }
    }
}
