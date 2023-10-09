using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class LoanOnlineDetailTemp : IEntity
    {
        public int Id {get; set ;}

        [Required]
        public User User { get; set; }

        public BookOnline BookOnline { get; set; }
   
        public int QuantityOfBooks { get; set; }
    }
}
