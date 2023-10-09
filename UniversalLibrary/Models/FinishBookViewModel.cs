using System;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Models
{
    public class FinishBookViewModel
    {       
        public int Id { get; set; }

        [Display(Name = "Return Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? LimitLoanDate { get; set; }
    }
}
