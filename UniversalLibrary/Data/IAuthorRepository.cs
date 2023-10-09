using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        public IQueryable GetAllWithUsers();

        //prepara a lista para inserir os authors na combobox do html -> addItemViewModel
        IEnumerable<SelectListItem> GetComboAuthors();
    }
}
