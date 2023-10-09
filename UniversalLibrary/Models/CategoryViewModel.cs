using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class CategoryViewModel : Category
    {
        //Propriedade para fazer o upload da imagem
        [Display(Name = "Icone of category")]
        public IFormFile ImageFile { get; set; }

    }
}
