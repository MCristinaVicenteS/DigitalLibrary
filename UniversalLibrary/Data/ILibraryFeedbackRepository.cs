using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public interface ILibraryFeedbackRepository
    {
        Task<IEnumerable<LibraryFeedback>> GetFeedbacksForHomePageAsync();
    }
}
