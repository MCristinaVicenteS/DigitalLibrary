using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        //****transfere para o "pai" - genericrepository -> para dp ser usado nos seus métodos****
        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        //****trás os livros com os users -> é um inerjoin das duas tabelas****
        public IQueryable GetAllWithUsers()
        {
            return _context.Books.Include(p => p.User);
        }
                    

        public IEnumerable<SelectListItem> GetComboBooks()
        {
            var list = _context.Books.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Book:",
                Value = "0"                    //****Para qd abrir a view n ter nenhum Book****
            });

            return list;                       //****No final retorna a lista****
        }

        public IEnumerable<SelectListItem> GetComboAuthors()
        {
            var list = _context.authors.Select(p => new SelectListItem
            {
                Text = p.FullName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select an Author:",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPublisher()
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

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            var list = _context.Categories.Select(p => new SelectListItem
            {
                Text = p.CategoryName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Category:",
                Value = "0"
            });

            return list;
        }

        public async Task<Book> GetBookWithAuthorsAsync(int id)
        {
            return await _context.Books
                .Include(a => a.Authors)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<SelectListItem> GetComboLibrary()
        {
            var list = _context.PhisicalLibraries.Select(p => new SelectListItem
            {
                Text = p.LibraryName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Phisical Library:",
                Value = "0"
            });

            return list;
        }
    }
}
