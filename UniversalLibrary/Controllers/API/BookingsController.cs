using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingsController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        //receber td os bookings do repository c o user associado
        [HttpGet]
        public ActionResult GetBooks()
        {
            return Ok(_bookingRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
