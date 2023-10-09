using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalLibrary.Data.Entities
{
    public class Reader : IEntity
    {
        public int Id { get; set; }


        //user que criou o reader
        public virtual User User { get; set; }

        public string UserId { get; set; }


        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }


        //[MaxLength(9, ErrorMessage = "The field {0} only can contain {1} characters lenght.")] //CORRIGIR ISTO -> COLOCAR MÁXIMO DE DÍGITOS
        [Display(Name = "Nif")]
        public int Nif { get; set; }


        [Display(Name = "Country")] //SERÁ SELECCIONADO POR COMBOBOX       
        public Country Country { get; set; }


        //VAI SER USADO NA COMBOBOX 
        //public int CityId { get; set; }


        [Display(Name = "City")]  //SERÁ SELECCIONADO POR COMBOBOX     
        public City City { get; set; }


        [Display(Name = "Address")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }


        public string PhoneNumber { get; set; }


        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
