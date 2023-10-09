using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {

        public IQueryable GetAllWithUsers();

        //prepara a lista para inserir as Categorias na combobox do html -> addItemViewModel
        IEnumerable<SelectListItem> GetComboCategories();

    }
}
