using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversalLibrary.Data;

namespace UniversalLibrary.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        //receber td os employees do repository c o user associado
        [HttpGet]
        public ActionResult GetBooks()
        {
            return Ok(_employeeRepository.GetAllWithUsers()); //dados em json -> através da função Ok
        }
    }
}
