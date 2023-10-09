using System.Collections.Generic;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class ShelfViewModel
    {
        public int Id { get; set; }

        public List<BookOnline> BookOnlines { get; set; }
    }
}
