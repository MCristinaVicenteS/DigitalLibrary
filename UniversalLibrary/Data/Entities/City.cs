using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class City : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "City")]
        [Required]
        public string CityName { get; set; }

    }
}
