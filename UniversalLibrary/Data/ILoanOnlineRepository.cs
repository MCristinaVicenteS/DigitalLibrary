using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Models;

namespace UniversalLibrary.Data
{
    public interface ILoanOnlineRepository : IGenericRepository<LoanOnline>
    {
        public IQueryable GetAllWithUsers();

        //****task para ir buscar as encomendas online associadas a um user****
        //public Task<List<LoanOnline>> GetLoanOnlineAsync(string userName);  //Mudei para a task debaixo -> confirmar
        Task<List<LoanOnline>> GetLoanOnlineAsync(string userName);


        //****método q devolve os dados temporários do Loan -> associa o Loan ao user -> LoanOnlineDetailTemp****
        //até confirmar os dados para a encomenda
        Task <List<LoanOnlineDetailTemp>> GetDetailTempAsync(string userName);

        //****método para adicionar os items à lista temporária****
        Task AddItemToLoanAsync(AddItemViewModel model, string userName);

        //método para apagar o item da lista temporária
        Task DeleteDetailTempAsync(int id);

        //método para confirmar o loan -> os dados passam para a lista definitiva
        //ou seja, passam da view create para a view index do controlador LoanOnline
        //e a tabela temporária desaparece
        Task<bool> ConfirmLoanAsync(string userName); 

        //task para asceitar a encomenda e dar a data de devolução do livro
        Task AcceptLoanOnline(FinishBookViewModel model);


        //método para adicionar o livro escolhido ao cart
        Task AddItemToCartLoanAsync(BookOnline id, string userName);

        Task<LoanOnline> GetLoanOnlineAsync(int id);

    }
}
