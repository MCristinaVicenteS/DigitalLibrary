using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;

        public LoansController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        //receber td os loans do repository c o user associado
        [HttpGet]
        public ActionResult GetBooks()
        {
            return Ok(_loanRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
