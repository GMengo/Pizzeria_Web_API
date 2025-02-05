using Microsoft.AspNetCore.Mvc;

namespace pizzeria_web_api.Controllers
{
    [Route("[controller]")]
    public class CategoriaController : ControllerBase
    {
        CategoriaRepository Repository;
        public CategoriaController(CategoriaRepository categoriaRepository) 
        { 
            CategoriaRepository Repository = categoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Repository.GetCategorie());
        }

        //public async Task<IActionResult> Get() => Ok(await Repository.GetCategorie()); //stessa cosa di quella sopra solo in formato lambda



    }
}
