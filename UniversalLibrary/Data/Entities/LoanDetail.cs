using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class LoanDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }


        public int QuantityOfBooks { get; set; }
    }
}
