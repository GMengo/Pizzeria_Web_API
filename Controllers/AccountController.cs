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

        public async Task<IActionResult> Register([FromBody] UtenteModel utente)
        {
            Boolean result = await _utenteService.RegisterAsync(utente);
            if (!result) 
            {
                return BadRequest(new { Message = "Registrazione fallita!" }); 
            }
            return Ok(new {Message = "Registrazione avvenuta con successo!"});
        }

    }
}
