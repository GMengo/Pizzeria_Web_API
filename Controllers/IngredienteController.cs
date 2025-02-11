using Microsoft.AspNetCore.Mvc;
using pizzeria_web_api.Models;
using pizzeria_web_api.Repositories;

namespace pizzeria_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class IngredienteController : Controller
    {
        private readonly IngredienteRepository _ingredienteRepository;

        public IngredienteController(IngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _ingredienteRepository.GetAllIngredientisAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetIngredienteById(int id)
        {
            try
            {
                Ingrediente ing = await _ingredienteRepository.GetIngredienteByIdAsync(id);
                if (ing == null)
                {
                    return NotFound();
                }
                return Ok(ing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] Ingrediente ingrediente)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return BadRequest(ModelState.Values);
                }
                ingrediente.Id = 0;
                int affectedRows = await _ingredienteRepository.InsertIngredient(ingrediente);
                return Ok(affectedRows);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] Ingrediente ingrediente)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return BadRequest(ModelState.Values);
                }
                int affectedRows = await _ingredienteRepository.UpdateIngredienteAsync(id, ingrediente);
                if (affectedRows == 0)
                {
                    return NotFound();
                }
                return Ok(affectedRows);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int affectedRows = await _ingredienteRepository.DeleteIngredienteAsync(id);
                if (affectedRows == 0)
                {
                    return NotFound();
                }
                return Ok(affectedRows);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}