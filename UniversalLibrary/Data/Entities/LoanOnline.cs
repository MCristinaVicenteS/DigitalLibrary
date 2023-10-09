using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class LoanOnline : IEntity
    {
        //INSERIR RESTRRIÇÃO - no layout -> só quem tem login feito

        public int Id { get; set; }


        [Display(Name = "User Name")]
        [Required]
        public User User { get; set; }

        //associar o Reader


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Loan Date")]
        [Required]
        public DateTime LoanDate { get; set; }


        //Data da encomenda segundo a hora local
        [Display(Name = "Local Loan Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime ? LoanDateLocal => this.LoanDate == null ? null : this.LoanDate.ToLocalTime();


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Return Date")]
        [Required]
        public DateTime ReturnDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Limit Loan Date")]
        [Required]
        public DateTime LimitLoadDate { get; set; }


        //Ligação c a tabela loanOnlineDetails -> od estão os books -> entram por aqui os livros q vêm da tabela temporária
        //Lista de enumerados
        public IEnumerable<LoanOnlineDetail> Items { get; set; }


        //qt de encomendas
        //se n houver itens escolhidos -> mostro zero; senão contam-se os itens
        //esta valor n fica na tabela -> é calculado na hora
        [Display(Name = "Quantity Of Loan Online")]
        public int QuantityOfLoanOnline => Items == null ? 0 : Items.Count();      //confirmar este método

        //Qt de livros por encomenda 
        //O valor n fica na tabela
        [Display(Name = "Quantity Of Books")]
        public int QuantityOfBooks => Items == null ? 0 : Items.Sum(i => i.QuantityOfBooks); //PONDERAR TIRAR ISTO
    }
}
