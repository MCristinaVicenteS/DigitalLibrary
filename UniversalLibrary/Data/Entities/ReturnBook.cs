using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class ReturnBook : IEntity
    {       
        public int Id { get; set; }

        //[Display(Name = "User Name")]
        [Required]
        public User User { get; set; }       // -> user logado 


        //public int UserId { get; set; }


        //public IEnumerable<SelectListItem> AvailableUsers { get; set; }  //o user q vai entregar o livro

        //public int ReaderId { get; set; }

        //public string ReaderNif { get; set; }

        public Loan loan { get; set; }

        public LoanDetail loanDetail { get; set; }

        //public IEnumerable<SelectListItem> AvailableBooks { get; set; }

        // CRIO UMA COMBOBOX DO USER.NIF E USER.FULLNAME -> ESCOLHO -> USAR FILTRO

        //[Required]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        //[Display(Name = "Book Delivery Date")]
        public DateTime ReturnDate { get; set; } //físico


        //[Display(Name = "Is the book Damaged?")]
        //[Required]
        public bool IsDamaged { get; set; }

        
        //[Display(Name = "Infraction Confirmation?")]
        //[Required]
        public bool PenaltyConfirmation { get; set; } 


        //[Display(Name = "Damage Description")]
        public string DamageDescription { get; set; }


        //[Display(Name = "Proof of Delivery")]
        public string Pdf { get; set; }


        [NotMapped]
        public IFormFile PdfFile { get; set; }


        //[Display(Name = "Return Information")]
        //[Required]
        //public string ReturnInformation => $"{loanDetail.Id} - {ReturnDate}";

    }
}
