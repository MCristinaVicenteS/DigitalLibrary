using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class LoanDetailTemp : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        public Book Book { get; set; }

        public int QuantityOfBooks { get; set; }
    }
}
