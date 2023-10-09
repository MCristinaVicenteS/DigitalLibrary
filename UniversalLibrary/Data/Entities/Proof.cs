using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class Proof : IEntity
    {    
        //comprovativo de pagamento, comprovativo de empréstimo, comprovativo de atraso

        public int Id { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }


        [Required]
        [Display(Name = "Email Proof")]
        public string EmailNotification { get; set; } //Duvida - Ficará melhor bool? e consuante o resultado envia-se ou n mail


        [Required]
        [Display(Name = "Pdf Proof")]
        public string PdfFileDocument { get; set; } //Duvida - Ficará melhor bool? e consuante o resultado envia-se ou n pdf


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Proof Date")]
        public DateTime CreationDate { get; set; }  


        //DUVIDA
        public List<Booking> Bookings { get; set; } //Esta é a classe das reservas -> tiro, certo?


        [Display(Name = "Proof with Books Loaned")]
        [Required]
        public List<Book> Books { get; set; }


        //Métodos Enviar por email
        //public string SendEmailProofLoan(User user)
        //{
        //    return $"O cliente com o nome {user.FullName}, requesitou: {Books}, no dia {CreationDate}." +               
        //        $"Volte sempre à Universal Library";
        //}

        //public string SendEmailProofPayment(User user, Loan loan)
        //{
        //    return $"O cliente com o nome {user.FullName}, requesitou: {Books}, no dia {loan.LoanDate}." + 
        //        $"Pagou o valor em dívida no dia {CreationDate}" +
        //        $"Volte sempre à Universal Library";
        //}

        //public string SendEmailProofPenalty(User user, Loan loan, InfractionType infractionType, Penalty penalty)
        //{
        //    return $"O cliente com o nome {user.FullName}, requesitou: {Books}, no dia {loan.LoanDate}." +
        //        $"Foi-lhe associada a coima {penalty.FineAmount} pela infracção do tipo {infractionType.Id}" +
        //        $"Volte sempre à Universal Library";
        //}
    }
}
