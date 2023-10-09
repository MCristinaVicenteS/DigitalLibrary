using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class ReaderViewModel
    {
        public int Id { get; set; }
               

        public string UserId { get; set; }

        public Reader Reader { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [RegularExpression(@"\d{9}", ErrorMessage = "The field {0} only can contain 9 numbers lenght.")]
        [Required]
        [Display(Name = "Nif")]
        public int Nif { get; set; }
               


        //[Required]
        [MaxLength(100, ErrorMessage = "The Field {0} only can contain {1} characters lenght")]
        public string Address { get; set; }


        [MaxLength(20, ErrorMessage = "The Field {0} only can contain {1} characters lenght")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        //Propriedade para fazer o upload da imagem
        [Display(Name = "Reader Photo")]
        public IFormFile ImageFile { get; set; }


        public string Image { get; set; }

    }
}
