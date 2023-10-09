using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public IQueryable GetAllWithUsers();

        //Prepara a lista para inserir os empregados na combobox do Html -> AddItemViewModel
        //DVUDIA: QUEREM COMBOBOX? n sei se aqui ficará bem
        IEnumerable<SelectListItem> GetComboEmployees();
    }
}
