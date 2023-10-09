using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class AuthorViewModel 
    {
        public int Id { get; set; }

        //NOTA: inserir o user que criou o author
        [Display(Name = "User Name")]
        public User User { get; set; }

        public string ImageAuthor { get; set; }

        //Propriedade para fazer o upload da imagem
        [Display(Name = "Author photo")]
        public IFormFile ImageFile { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Birthday")]
        public DateTime DateOfBirth { get; set; }


        //[RegularExpression(@"\d{9}", ErrorMessage = "The field {0} only can contain 9 numbers lenght.")]
        [Required]
        [Display(Name = "Nif")]
        public int Nif { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Country")]
        [Required]
        public string Country { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Nationality")]
        [Required]
        public string Nationality { get; set; }


        [MaxLength(150, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Biography")]
        public string Biography { get; set; }


        [MaxLength(150, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Major Works")]
        public string MajorWorks { get; set; }


        [MaxLength(100, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Awards and Recognitions")]
        public string AwardsAndRecognitions { get; set; }


        [MaxLength(100, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Other Details")]
        public string OtherDetails { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";



        
    }
}
