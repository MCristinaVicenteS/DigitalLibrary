using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Country")]
        [Required]
        public string CountryName { get; set; }

        [Display(Name = "Flag")]
        [Required]
        public string Flag { get; set; }
    }
}
