using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface IBookPublisherRepository : IGenericRepository<BookPublisher>
    {
        public IQueryable GetAllWithUsers();

        //Prepara a lista para inserir as editoras na combobox do Html -> AddItemViewModel
        //-> QUEREM COMBOBOX?
        IEnumerable<SelectListItem> GetComboBookPublishers();
    }
}
