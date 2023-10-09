using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly DataContext _context;

        //transfere para o GenericRepository -> para dp ser usado nos seus métodos
        public AuthorRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.authors.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboAuthors()
        {
            var list = _context.authors.Select(p => new SelectListItem
            {
                Text = p.FirstName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Author:",
                Value = "0"
            });

            return list;
        }
    }
}
