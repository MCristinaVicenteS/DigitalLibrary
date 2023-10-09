using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class Recomendation : IEntity
    {
       
        public int Id { get; set; }

        public User User { get; set; }

        [Display(Name = "Recomendations")]
        [Required]
        public string Comments { get; set; } 
    }
}
