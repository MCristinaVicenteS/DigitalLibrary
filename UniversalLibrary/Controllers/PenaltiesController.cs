using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Controllers
{
    public class PenaltiesController : Controller
    {
        private readonly DataContext _context;

        public PenaltiesController(DataContext context)
        {
            _context = context;
        }

        // GET: Penalties
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Penalties.Include(p => p.Book).Include(p => p.InfractionLocation).Include(p => p.User);
            return View(await dataContext.ToListAsync());

        }

        // GET: Penalties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalties
                .Include(p => p.Book)
                .Include(p => p.InfractionLocation)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // GET: Penalties/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["InfractionLocationId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Penalties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InfractionType,InfractionDateTime,InfractionLocationId,UserId,BookId,FineAmount,PaymentDate,PaymentStatus")] Penalty penalty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(penalty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", penalty.BookId);
            ViewData["InfractionLocationId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName", penalty.InfractionLocationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", penalty.UserId);
            return View(penalty);
        }

        // GET: Penalties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalties.FindAsync(id);
            if (penalty == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", penalty.BookId);
            ViewData["InfractionLocationId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName", penalty.InfractionLocationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", penalty.UserId);
            return View(penalty);
        }

        // POST: Penalties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InfractionType,InfractionDateTime,InfractionLocationId,UserId,BookId,FineAmount,PaymentDate,PaymentStatus")] Penalty penalty)
        {
            if (id != penalty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(penalty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PenaltyExists(penalty.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", penalty.BookId);
            ViewData["InfractionLocationId"] = new SelectList(_context.PhisicalLibraries, "Id", "LibraryName", penalty.InfractionLocationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", penalty.UserId);
            return View(penalty);
        }

        // GET: Penalties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _context.Penalties
                .Include(p => p.Book)
                .Include(p => p.InfractionLocation)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // POST: Penalties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var penalty = await _context.Penalties.FindAsync(id);
            _context.Penalties.Remove(penalty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PenaltyExists(int id)
        {
            return _context.Penalties.Any(e => e.Id == id);
        }
    }
}
