using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzeria_web_api.Models;
using pizzeria_web_api.Services;
using System.Security.Claims;

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
            try
            {
                Boolean result = await _utenteService.RegisterAsync(utente);
                if (!result)
                {
                    return BadRequest(new { Message = "Registrazione fallita! La password deve contenere almeno 8 caratteri, un numero e una lettera maiuscola" });
                }
                return Ok(new { Message = "Registrazione avvenuta con successo!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Login([FromBody] UtenteModel utente)
        {
            string token = await _jwtAuthenticationService.Authenticate(utente.Email, utente.Password);
            if (token == null)
            {
                return Unauthorized("Credenziali non valide");
            }

            // aggiunta del cookie per rendere automatico il riconoscimento di chi è loggato, senza doverlo testare manualmente inserendo il token, assegnando questo compito al broswer 
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(_jwtAuthenticationService._jwtSettings.DurationInMinutes)
            });


            return Ok(new
            {
                Token = token,
                ExpirationUtc = DateTime.UtcNow.AddMinutes(_jwtAuthenticationService._jwtSettings.DurationInMinutes)
            });
        }

        [HttpGet("GetUserRoles/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserRolesById(int userId)
        {
            try
            {
                var ruoli = await _utenteService.GetUserRolesAsync(userId);
                return Ok(ruoli);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Lettura automatica dal JWT nei cookie
        [HttpGet("GetMyRoles")]
        [Authorize]
        public async Task<IActionResult> GetMyRoles()
        {
            try
            {
                // Estrazione ID utente dal JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Utente comune");
                }

                var ruoli = await _utenteService.GetUserRolesAsync(userId);
                return Ok(ruoli);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[Action]")]
        [Authorize] 
        public IActionResult Logout()
        {
            // Cancella il cookie "jwt" inviando un'istruzione al client
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                Secure = false, 
                SameSite = SameSiteMode.Lax
            });

            return Ok(new { Message = "Logout effettuato con successo!" });
        }
    }
}
