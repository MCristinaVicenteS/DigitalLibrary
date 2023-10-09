using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace UniversalLibrary.Models
{
    public class FinishDateBookingViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Limit Date to Take the Book")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? LimitDate { get; set; }
    }
}
