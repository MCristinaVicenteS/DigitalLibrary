using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        //receber td os authors do repository c o user associado
        [HttpGet]
        public ActionResult GetAuthors()
        {
            return Ok(_authorRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
