using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;

        //transfere para o "pai" - genericrepository -> para dp ser usado nos seus métodos
        public EmployeeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        //trás os empregados com os users -> é um inerjoin das duas tabelas -> os users q criaram os empregados -> o admin
        public IQueryable GetAllWithUsers()
        {
            return _context.Employees.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboEmployees()
        {
            var list = _context.Employees.Select(p => new SelectListItem
            {
                Text = p.FirstName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Employee:",
                Value = "0"
            });

            return list;
        }
    }
}
