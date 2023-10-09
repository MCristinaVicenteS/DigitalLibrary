using Org.BouncyCastle.Asn1.Misc;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class LoanOnlineDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public BookOnline BookOnline { get; set; }

        
        public int QuantityOfBooks { get; set; }
        
    }
}
