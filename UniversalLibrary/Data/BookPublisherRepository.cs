using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class BookPublisherRepository : GenericRepository<BookPublisher>, IBookPublisherRepository
    {
        private readonly DataContext _context;

        //transfere para o "pai" - genericrepository -> para dp ser usado nos seus métodos
        public BookPublisherRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        //trás as editoras com os users -> é um inerjoin das duas tabelas
        public IQueryable GetAllWithUsers()
        {
            return _context.BookPublishers.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboBookPublishers()
        {
            var list = _context.BookPublishers.Select(p => new SelectListItem
            {
                Text = p.PublisherName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Book Publisher:",
                Value = "0"
            });

            return list;
        }

    }
}
