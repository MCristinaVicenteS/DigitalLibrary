using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class BookPublisherViewModel : BookPublisher
    {
        //Propriedade para fazer o upload da imagem
        [Display(Name = "Logo")]
        public IFormFile ImageFile { get; set; }
    }
}
