using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class ReaderRepository : GenericRepository<Reader>, IReaderRepository
    {
        private readonly DataContext _context;

        public ReaderRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        //public IQueryable GetAllWithUsers()
        //{
        //    return _context.Readers.Include(p => p.User);
        //}


        //public IEnumerable<SelectListItem> GetComboReaders()
        //{
        //    var list = _context.Readers.Select(p => new SelectListItem
        //    {
        //        Text = p.Nif.ToString(),
        //        Value = p.Id.ToString(),
        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "Select a Physical Reader:",
        //        Value = "0"
        //    });

        //    return list;
        //}
    }
}
