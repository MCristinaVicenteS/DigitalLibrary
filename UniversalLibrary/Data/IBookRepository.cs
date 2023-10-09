using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    //inserir no Startup
    public interface IBookRepository : IGenericRepository<Book>
    {
        public IQueryable GetAllWithUsers();               

        //****método q gera uma lista de books e carrega a combobox (está no AddItemViewModel) c os books (estão no bookRepository)****
        IEnumerable<SelectListItem> GetComboBooks();

        IEnumerable<SelectListItem> GetComboAuthors();

        IEnumerable<SelectListItem> GetComboPublisher();

        IEnumerable<SelectListItem> GetComboCategories();

        IEnumerable<SelectListItem> GetComboLibrary();


        //através do id devolve o objecto book -> com os authors q tem
        Task<Book> GetBookWithAuthorsAsync(int id);
    }
}
