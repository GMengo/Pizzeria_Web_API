using System.Reflection.Metadata;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using pizzeria_web_api.Models;
using pizzeria_web_api.Repositories;
using System.Net;

namespace pizzeria_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzaController : ControllerBase
    {

        private PizzaRepository PizzaRepository { get; set; }
        private ICustomLogger _logger;

        public PizzaController(ICustomLogger logger, PizzaRepository pizzaRepository)
        {
            PizzaRepository = pizzaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? nome) {
            _logger.WriteStartingLogWithHttpInfo(HttpContext, $"Nome parametro: {nome ?? "null"}");

            try
            {
                if (nome == null)
                {
                    List<Pizza> result = (await PizzaRepository.GetAllPizza());
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Restituiti {result.Count} risultati");
                    return Ok(result);
                    
                    
                }
                else
                {
                    List<Pizza> result = (await PizzaRepository.GetPizzaByName(nome));
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Restituiti {result.Count} risultati per nome '{nome}'");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: {ex.Message}", (HttpStatusCode)400);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetPizzaById(int id)
        {
            _logger.WriteStartingLogWithHttpInfo(HttpContext);
            if (id <= 0)
            {
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: Inserire un id maggiore di 0", (HttpStatusCode)400);
                return BadRequest("Inserire un id maggiore di 0");
            };

            try
            {
                Pizza p = await PizzaRepository.GetPizzaByIdAsync(id);

                if (p != null)
                {
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Pizza con l' Id: {id} trovata");
                    return Ok(p);
                }
                else
                {
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Pizza con l' Id: {id} non trovata", (HttpStatusCode)404);
                    return NotFound($"Non � stata trovata nessuna pizza con l' id: {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: {ex.Message}", (HttpStatusCode)400);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        //creare una pizza passando un json nel body della richiesta
        public async Task<IActionResult> CreatePizza([FromBody] Pizza p)
        {
            _logger.WriteStartingLogWithHttpInfo(HttpContext);
            try
            {
                if (ModelState.IsValid == false)
                {
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Dati non validi: {ModelState.Values}", (HttpStatusCode)400);
                    return BadRequest($"Dati non validi: {ModelState.Values}");
                }
                (int affectedRows, Pizza createdPizza) createdTupla = await PizzaRepository.CreatePizza(p);
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"� stata aggiunta: {createdTupla.affectedRows} Pizza al DB", (HttpStatusCode)201);
                return Created($"� stata aggiunta: {createdTupla.affectedRows} Pizza al DB", createdTupla.createdPizza);
            }
            catch (Exception e)
            {
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: {e.Message}", (HttpStatusCode)400);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdatePizza(int id,[FromBody] Pizza updatedPizza)
        {
            _logger.WriteStartingLogWithHttpInfo(HttpContext);
            try
            {
                if (ModelState.IsValid == false)
                {
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Dati non validi: {ModelState.Values}", (HttpStatusCode)400);
                    return BadRequest($"Dati non validi: {ModelState.Values}");
                }
                int affectedRows = await PizzaRepository.UpdatePizza(id, updatedPizza);
                if (affectedRows == 0)
                {
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Pizza con l' Id: {id} non trovata", (HttpStatusCode)404);
                    return NotFound();
                }
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"� stata modificata la Pizza con Id: {id}", (HttpStatusCode)201);
                return Ok(affectedRows);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePizza(int id)
        {
            _logger.WriteStartingLogWithHttpInfo(HttpContext);
            try
            {
                int affectedRows = await PizzaRepository.DeletePizza(id);
                if (affectedRows == 0)
                {
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Pizza con l' Id: {id} non trovata", (HttpStatusCode)404);
                    return NotFound();
                }
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"� stata eliminata {affectedRows} Pizza");
                return Ok(affectedRows);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: {e.Message}");
                return BadRequest(e.Message);
            }
        }

    }
}
