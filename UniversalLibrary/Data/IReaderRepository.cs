using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface IReaderRepository : IGenericRepository<Reader>
    {
        //public IQueryable GetAllWithUsers();

        //IEnumerable<SelectListItem> GetComboReaders();
    }
}
