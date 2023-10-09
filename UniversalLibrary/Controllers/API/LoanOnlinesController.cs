using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanOnlinesController : ControllerBase
    {
        private readonly ILoanOnlineRepository _loanOnlineRepository;

        public LoanOnlinesController(ILoanOnlineRepository loanOnlineRepository)
        {
            _loanOnlineRepository = loanOnlineRepository;
        }

        //receber td os loanOnlines do repository c o user associado
        [HttpGet]
        public ActionResult GetBooks()
        {
            return Ok(_loanOnlineRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
