using System.Reflection.Metadata;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;

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
            //_logger.WriteLog($"[GET] /Pizza - Nome: {nome ?? "null"}");
            _logger.WriteStartingLogWithHttpInfo(HttpContext, $"Nome parametro: {nome ?? "null"}");

            try
            {
                if (nome == null)
                {
                    List<Pizza> result = (await PizzaRepository.GetAllPizza());
                    //_logger.WriteLog($"[GET] /Pizza - 200 OK - Restituiti {result.Count} risultati");
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Restituiti {result.Count} risultati");
                    return Ok(result);
                    
                    
                }
                else
                {
                    List<Pizza> result = (await PizzaRepository.GetAllPizza());
                    _logger.WriteResultLogWithHttpInfo(HttpContext, $"Restituiti {result.Count} risultati per nome '{nome}'");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                _logger.WriteResultLogWithHttpInfo(HttpContext, $"Errore: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetPizzaById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Inserire un id maggiore di 0");
            };

            try
            {
                Pizza p = await PizzaRepository.GetPizzaByIdAsync(id);

                if (p != null)
                {
                    return Ok(p);
                }
                else
                {
                    return NotFound($"Non è stata trovata nessuna pizza con l' id: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        ////creare una pizza passando i parametri 
        //public async Task<IActionResult> CreatePizzaParams([FromQuery]string nome, string descrizione, decimal prezzo)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid == false)
        //        {
        //            return BadRequest($"Dati non validi: {ModelState.Values}");
        //        }
        //        int affectedRows = await PizzaRepository.CreatePizzaParams(nome, descrizione, prezzo);
        //        return Ok(affectedRows);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPost]
        //creare una pizza passando un json nel body della richiesta
        public async Task<IActionResult> CreatePizzaBody([FromBody] Pizza p)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return BadRequest($"Dati non validi: {ModelState.Values}");
                }
                (int affectedRows, Pizza createdPizza) createdTupla = await PizzaRepository.CreatePizzaBody(p);
                //return StatusCode(201,$"è stata aggiunta: {affectedRows} Pizza al DB con il nome: {p.Nome}");
                return Created($"è stata aggiunta: {createdTupla.affectedRows} Pizza al DB", createdTupla.createdPizza);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdatePizza(int id,[FromBody] Pizza updatedPizza)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return BadRequest($"Dati non validi: {ModelState.Values}");
                }
                int affectedRows = await PizzaRepository.UpdatePizza(id, updatedPizza);
                if (affectedRows == 0)
                {
                    return NotFound();
                }
                
                return Ok(affectedRows);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePizza(int id)
        {
            try
            {
                int affectedRows = await PizzaRepository.DeletePizza(id);
                if (affectedRows == 0)
                {
                    return NotFound();
                }
                return Ok(affectedRows);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
