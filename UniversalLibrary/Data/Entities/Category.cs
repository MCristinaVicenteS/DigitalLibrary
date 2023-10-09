using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        public User User { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string CategoryName { get; set; }

        
    }
}
