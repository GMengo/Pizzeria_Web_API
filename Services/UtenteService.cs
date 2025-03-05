﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using pizzeria_web_api.Models;
using System.Data;

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
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            string ricercaUtente = "SELECT * FROM Utente where Email = @Email";
            using (SqlCommand commandRicerca = new SqlCommand(ricercaUtente, connection))
            {
                commandRicerca.Parameters.AddWithValue("@Email", utente.Email);
                SqlDataReader reader = await commandRicerca.ExecuteReaderAsync();
                if (reader.Read())
                    throw (new Exception(message: "Esiste già un utente registrato con l' email inserita"));
                await reader.CloseAsync();
            }
            bool validatoreNumero = false;
            bool validatoreMaiuscola = false;

            if (utente.Password.Length < 8)
                return false;

            foreach (char carattere in utente.Password)
            {
                try
                {
                    // va fatto perchè la conversione di una stringa in char è sempre vera perchè restituisce il suo corrispetivo numerico
                    string daCharaString = Convert.ToString(carattere);
                    int tester = Convert.ToInt32(daCharaString);
                    validatoreNumero = true;
                }
                catch (Exception ex) { }

                if (char.IsLetter(carattere) && carattere.ToString() == carattere.ToString().ToUpper())
                {
                    validatoreMaiuscola = true;
                }
            }
            if (validatoreMaiuscola == false || validatoreNumero == false)
            {
                return false;
            }

            string passwordHash = _passwordHasher.HashPassword(utente, utente.Password);

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

        //  andare a vedere -> .\SQLQuery_creazione_e_popolamento_DB.sql per spiegazione gestione ruoli
        public async Task<List<string>> GetUserRolesAsync(int utenteId)
        {
            List<string> ruoli = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(
                "SELECT r.Nome " +
                "FROM Ruolo r " +
                "INNER JOIN UtenteRuolo ur ON r.Id = ur.RuoloId " +
                "WHERE ur.UtenteId = @UtenteId", connection);
                command.Parameters.AddWithValue("@UtenteId", utenteId);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ruoli.Add(reader.GetString(0));
                }
            }
            return ruoli;
        }

        public async Task<Utente> GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "select * from utente where id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Utente u = new Utente();
                    u.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    u.Email = reader.GetString(reader.GetOrdinal("email"));
                    return u;
                }
            }
            return null;
        }

        public async Task<Utente> GetUserByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "select * from utente where email = @email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Utente u = new Utente();
                    u.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    u.Email = reader.GetString(reader.GetOrdinal("email"));
                    return u;
                }
            }
            return null;
        }

    }
}
