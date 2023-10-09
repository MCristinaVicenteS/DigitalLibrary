using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using UniversalLibrary.Data;

namespace UniversalLibrary.Models
{
    public class ImageViewModel:IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
