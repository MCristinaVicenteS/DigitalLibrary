using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class EmployeeViewModel : Employee
    {
        //Propriedade para fazer o upload da imagem
        [Display(Name = "Employee Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
