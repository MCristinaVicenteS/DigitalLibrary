using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class Penalty : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Infraction Type (Please Select Infraction)")]
        [Required]
        public InfractionType InfractionType { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Infraction Date")]
        public DateTime InfractionDateTime { get; set; }


        [Display(Name = "Infraction Library (Please Select Library Name)")]
        public int InfractionLocationId { get; set; }

        //[Display(Name = "Infraction Location")]
        //[Required]
        public virtual PhisicalLibrary InfractionLocation { get; set; }



        [Display(Name = "User Name (Please Select a user)")]
        public string UserId { get; set; }
        public virtual User User { get; set; }


        [Display(Name = "Book (Please Select a book)")]
        public int BookId { get; set; }

        //[Display(Name = "Book")]
        //[Required]
        public virtual Book Book { get; set; }


        [Display(Name = "Price to Pay")]
        [Required]
        public decimal FineAmount { get; set; }
               
                
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Payment Date")]
        [Required]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Payment Status")]
        [Required]
        public bool PaymentStatus { get; set; }

    }
}
