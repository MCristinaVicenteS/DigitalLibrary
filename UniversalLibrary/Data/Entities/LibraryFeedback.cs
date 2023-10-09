using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace UniversalLibrary.Data.Entities
{
    public class LibraryFeedback
    {
        public int Id { get; set; }

        [Display(Name = "Library")]
        [Required]
        public int PhisicalLibraryId { get; set; }


        public virtual PhisicalLibrary PhisicalLibrary { get; set; }


        [Required]
        public string Comment { get; set; }


        [Required]
        public int Rating { get; set; }


        [Required]
        public DateTime CreatedDate { get; set; }


        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}


