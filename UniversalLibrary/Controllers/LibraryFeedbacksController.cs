
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Data;
using System.Linq;


namespace UniversalLibrary.Controllers
{
    public class LibraryFeedbacksController : Controller
    {
        private readonly DataContext _context;

        public LibraryFeedbacksController(DataContext context)
        {
            _context = context;
        }

        // GET: LibraryFeedbacks
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.LibraryFeedbacks.Include(l => l.PhisicalLibrary).Include(l => l.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: LibraryFeedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryFeedback = await _context.LibraryFeedbacks
                .Include(l => l.PhisicalLibrary)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryFeedback == null)
            {
                return NotFound();
            }

            return View(libraryFeedback);
        }

        // GET: LibraryFeedbacks/Create
        public IActionResult Create()
        {
            ViewData["PhisicalLibraryId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: LibraryFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhisicalLibraryId,Comment,Rating,CreatedDate,UserId")] LibraryFeedback libraryFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhisicalLibraryId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName", libraryFeedback.PhisicalLibraryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", libraryFeedback.UserId);
            return View(libraryFeedback);
        }

        // GET: LibraryFeedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryFeedback = await _context.LibraryFeedbacks.FindAsync(id);
            if (libraryFeedback == null)
            {
                return NotFound();
            }
            ViewData["PhisicalLibraryId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName", libraryFeedback.PhisicalLibraryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", libraryFeedback.UserId);
            return View(libraryFeedback);
        }

        // POST: LibraryFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhisicalLibraryId,Comment,Rating,CreatedDate,UserId")] LibraryFeedback libraryFeedback)
        {
            if (id != libraryFeedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryFeedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryFeedbackExists(libraryFeedback.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhisicalLibraryId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibyraryName", libraryFeedback.PhisicalLibraryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", libraryFeedback.UserId);
            return View(libraryFeedback);
        }

        // GET: LibraryFeedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryFeedback = await _context.LibraryFeedbacks
                .Include(l => l.PhisicalLibrary)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryFeedback == null)
            {
                return NotFound();
            }

            return View(libraryFeedback);
        }

        // POST: LibraryFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libraryFeedback = await _context.LibraryFeedbacks.FindAsync(id);
            _context.LibraryFeedbacks.Remove(libraryFeedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryFeedbackExists(int id)
        {
            return _context.LibraryFeedbacks.Any(e => e.Id == id);
        }
    }
}
