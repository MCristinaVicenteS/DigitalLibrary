using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UniversalLibrary.Models
{
    public class AddItemToCartViewModel
    {
        [Display(Name = "Book")]
        public int BookId { get; set; }

        public IEnumerable<SelectListItem> Books { get; set; }
    }
}
