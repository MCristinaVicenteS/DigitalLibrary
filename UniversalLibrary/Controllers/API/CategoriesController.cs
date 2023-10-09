using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        //receber td as categorias do repository c o user associado
        [HttpGet]
        public ActionResult GetCategories()
        {
            return Ok(_categoryRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
