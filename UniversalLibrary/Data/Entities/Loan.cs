using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UniversalLibrary.Data.Entities
{
    //EMPRESTIMO FÍSICO

    //INSERIR RESTRRIÇÃO - no layout -> só quem tem login e tem o role empregado ou admin

    public class Loan : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        [Required]
        public User User { get; set; } 

        //Associar o Employee

        //associar o Reader

        
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Loan Date")]
        [Required]
        public DateTime LoanDate { get; set; }

        //Data da encomenda segundo a hora local
        [Display(Name = "Loan Date Local")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? LoanDateLocal => this.LoanDate == null ? null : this.LoanDate.ToLocalTime();


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Return Date")]
        [Required]
        public DateTime ReturnDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Limit Loan Date")]
        [Required]
        public DateTime LimitLoadDate { get; set; }


        //Ligação c a tabela loanDetails -> od estão os books -> entram por aqui os livros q vêm da tabela temporária
        //Lista de enumerados
        public IEnumerable<LoanDetail> Items { get; set; }


        //se n houver itens escolhidos -> mostro zero; senão contam-se os itens
        //esta valor n fica na tabela -> é calculado na hora
        [Display(Name = "Quantity Of Loan Online")]
        public int QuantityOfLoan => Items == null ? 0 : Items.Count();


        //O valor n fica na tabela
        //Qt de livros por encomenda
        [Display(Name = "Quantity Of Books")]
        public int QuantityOfBooks => Items == null ? 0 : Items.Sum(i => i.QuantityOfBooks);
    }
}
