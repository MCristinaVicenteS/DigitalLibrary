using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookPublishersController : Controller
    {
        private readonly IBookPublisherRepository _bookPublisherRepository;

        public BookPublishersController(IBookPublisherRepository bookPublisherRepository)
        {
            _bookPublisherRepository = bookPublisherRepository;
        }

        //receber td os bookPublishers do repository c o user associado
        [HttpGet]
        public ActionResult GetBookPublishers()
        {
            return Ok(_bookPublisherRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
