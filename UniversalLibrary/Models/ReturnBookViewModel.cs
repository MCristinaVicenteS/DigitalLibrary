using System.ComponentModel.DataAnnotations;
using System;
using UniversalLibrary.Data.Entities;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using Syncfusion.Compression.Zip;

namespace UniversalLibrary.Models
{
    public class ReturnBookViewModel
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        //public IEnumerable<SelectListItem> AvailableReaders { get; set; }


        //public string ReaderId { get; set; }

        //public string ReaderNif { get; set; }


        public Loan loan { get; set; }

        //Loan date

        //public IEnumerable<SelectListItem> AvailableLoans { get; set; }

        //public int LoanId { get; set; }

        public LoanDetail loanDetail { get; set; }       

        //public IEnumerable<SelectListItem> AvailableBooks { get; set; }

        //public int BookId { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Book Delivery Date")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Is the book Damaged?")]
        [Required]
        public bool IsDamaged { get; set; }

        [Display(Name = "Infraction Confirmation?")]
        [Required]
        public bool PenaltyConfirmation { get; set; }


        [Display(Name = "Damage Description")]
        public string DamageDescription { get; set; }


        [Display(Name = "Proof of Delivery")]
        public string Pdf { get; set; }


        [NotMapped]
        public IFormFile PdfFile { get; set; }
    }
}
