using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface IReturnBookRepository : IGenericRepository<ReturnBook>
    {
        public IQueryable GetAllWithUsers();

        //IEnumerable<SelectListItem> GetComboUsers();

        IEnumerable<SelectListItem> GetComboBooks();

        IEnumerable<SelectListItem> GetComboLoans();

        //****task para ir buscar as encomendas associadas a um user****
        Task<IQueryable<Loan>> GetLoanAsync(string userName);
    }
}
