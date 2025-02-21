using System.ComponentModel.DataAnnotations;

namespace pizzeria_web_api.Models
{
    public class Utente
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
    public class UtenteModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
