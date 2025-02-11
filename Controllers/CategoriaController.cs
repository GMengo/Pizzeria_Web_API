using Microsoft.AspNetCore.Mvc;
using pizzeria_web_api.Repositories;

namespace pizzeria_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriaController : ControllerBase
    {
        private CategoriaRepository Repository;
        public CategoriaController(CategoriaRepository categoriaRepository) 
        { 
            Repository = categoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Repository.GetCategorie());
        }

        //public async Task<IActionResult> Get() => Ok(await Repository.GetCategorie()); //stessa cosa di quella sopra solo in formato lambda



    }
}
