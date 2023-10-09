using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class LoanViewModel : Loan
    {

        [Display(Name = "Loan")]  //PONDERAR tirar
        public int LoanId { get; set; }

        [Display(Name = "Book")]
        [Range(1, int.MaxValue,ErrorMessage = "Select a Book.")]
        public int BookId { get; set; }

        public IEnumerable<SelectListItem> Books { get; set; }

    }
}
