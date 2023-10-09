using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class BookDetailsViewModel : BookViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }


        //Propriedade para fazer o upload da imagem
        [Display(Name = "Book Cover")]
        public IFormFile ImageFile { get; set; }


        public string Image { get; set; }

        //adiconar a conecção Book com Author
        public IEnumerable <Author> Authors { get; set; }
    }
}
