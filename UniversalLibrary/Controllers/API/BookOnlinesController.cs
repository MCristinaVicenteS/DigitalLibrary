using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookOnlinesController : Controller
    {
        private readonly IBookOnlineRepository _bookOnlineRepository;

        public BookOnlinesController(IBookOnlineRepository bookOnlineRepository)
        {
            _bookOnlineRepository = bookOnlineRepository;
        }

        [HttpGet]
        public ActionResult GetBookOnlines() 
        {
            return Ok(_bookOnlineRepository.GetAllWithUsers());
        }
    }
}
