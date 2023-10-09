using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]        
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Required]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Required]
        public string LastName { get; set; }


        //[MaxLength(9, ErrorMessage = "The field {0} only can contain {1} characters lenght.")] //CORRIGIR ISTO -> COLOCAR MÁXIMO DE DÍGITOS
        [Display(Name = "Nif")]
        //[Required]
        public int Nif { get; set; }


        [Display(Name = "Country")] //VAI SER SELECCIONADO POR COMBOBOX        
        //[Required]
        public Country Country { get; set; } //DUVIDA ->Segundo o Rafael -> a pesquisa é feita só pela cidade


        //VAI SER USADO NA COMBOBOX 
        //public int CityId { get; set; }


        [Display(Name = "City")]  //VAI SER SELECCIONADO POR COMBOBOX     
        //[Required]
        public City City { get; set; } 


        [Display(Name = "Address")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        //[Required]
        public string Address { get; set; }
               

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
