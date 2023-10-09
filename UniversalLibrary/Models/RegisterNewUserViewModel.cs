using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UniversalLibrary.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [RegularExpression(@"\d{9}", ErrorMessage = "The field {0} only can contain 9 numbers lenght.")]
        //[Required]
        [Display(Name = "Nif")]        
        public int Nif { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        //[Required]
        [MaxLength(100, ErrorMessage = "The Field {0} only can contain {1} characters lenght")]
        public string Address { get; set; }


        [MaxLength(20, ErrorMessage = "The Field {0} only can contain {1} characters lenght")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        //[Display(Name = "City")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
        //public int CityId { get; set; } //FICARÁ NA COMBOBOX

        //FICARÁ NA COMBOBOX
        /*public IEnumerable<SelectListItem> Cities { get; set; }*/ //DUVIDA - FICA LIST OU IENUMERABLE?


        //[Display(Name = "Country")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        //public int CountryId { get; set; } //FICARÁ NA COMBOBOX


        //FICARÁ NA COMBOBOX
        /* public IEnumerable<SelectListItem> Countries { get; set; }*/ //DUVIDA - FICA LIST OU IENUMERABLE?

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
