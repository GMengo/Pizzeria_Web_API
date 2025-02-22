using Microsoft.AspNetCore.Mvc;
using pizzeria_web_api.Models;
using pizzeria_web_api.Services;

namespace pizzeria_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly JwtAuthenticationService _jwtAuthenticationService;
        private readonly UtenteService _utenteService;

        public AccountController(JwtAuthenticationService jwtAuthenticationService, UtenteService utenteService)
        {
            _jwtAuthenticationService = jwtAuthenticationService;
            _utenteService = utenteService;
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Register([FromBody] UtenteModel utente)
        {
            Boolean result = await _utenteService.RegisterAsync(utente);
            if (!result) 
            {
                return BadRequest(new { Message = "Registrazione fallita!" }); 
            }
            return Ok(new {Message = "Registrazione avvenuta con successo!"});
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Login([FromBody] UtenteModel utente)
        {
            string token = await _jwtAuthenticationService.Authenticate(utente.Email, utente.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Token = token,
                ExpirationUtc = DateTime.UtcNow.AddMinutes(_jwtAuthenticationService._jwtSettings.DurationInMinutes)
            });
        }



    }
}
