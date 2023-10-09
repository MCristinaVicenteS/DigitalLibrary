using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface IBookOnlineRepository : IGenericRepository<BookOnline>
    {
        public IQueryable GetAllWithUsers();

        //****método q gera uma lista de books e carrega a combobox (está no AddItemViewModel) c os books (estão no bookRepository)****
        IEnumerable<SelectListItem> GetComboBooks();

        IEnumerable<SelectListItem> GetComboAuthors();

        IEnumerable<SelectListItem> GetComboPublisher();

        IEnumerable<SelectListItem> GetComboCategories();

        //através do id devolve o objecto book -> com os authors q tem
        Task<BookOnline> GetBookWithAuthorsAsync(int id);
    }
}
