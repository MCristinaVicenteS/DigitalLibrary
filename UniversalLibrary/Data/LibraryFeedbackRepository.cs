using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class LibraryFeedbackRepository : ILibraryFeedbackRepository
    {
        private readonly DataContext _context;

        public LibraryFeedbackRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<LibraryFeedback>> GetFeedbacksForHomePageAsync()
        {
            var feedbacks = await _context.LibraryFeedbacks
                .Include(l => l.PhisicalLibrary)
                .Include(l => l.User)
                .ToListAsync();



            return feedbacks;
        }
    }
}
