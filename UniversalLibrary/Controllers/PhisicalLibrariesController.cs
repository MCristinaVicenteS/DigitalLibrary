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
    public class PhisicalLibrariesController : Controller
    {
        private readonly DataContext _context;

        public PhisicalLibrariesController(DataContext context)
        {
            _context = context;
        }

        // GET: PhisicalLibraries
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhisicalLibraries.ToListAsync());
        }

        // GET: PhisicalLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phisicalLibrary = await _context.PhisicalLibraries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phisicalLibrary == null)
            {
                return NotFound();
            }

            return View(phisicalLibrary);
        }

        // GET: PhisicalLibraries/Create
        public IActionResult Create()
        {            
            return View();
        }

        // POST: PhisicalLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhisicalLibrary phisicalLibrary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phisicalLibrary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phisicalLibrary);
        }

        // GET: PhisicalLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phisicalLibrary = await _context.PhisicalLibraries.FindAsync(id);
            if (phisicalLibrary == null)
            {
                return NotFound();
            }
            return View(phisicalLibrary);
        }

        // POST: PhisicalLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LibraryName,FoundationDate,Director,Stock,PhoneNumber,Address,Email,WebSite,OpenTime,CloseTime,WorkingDay,GoogleMap")] PhisicalLibrary phisicalLibrary)
        {
            if (id != phisicalLibrary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phisicalLibrary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhisicalLibraryExists(phisicalLibrary.Id))
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
            return View(phisicalLibrary);
        }

        // GET: PhisicalLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phisicalLibrary = await _context.PhisicalLibraries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phisicalLibrary == null)
            {
                return NotFound();
            }

            return View(phisicalLibrary);
        }

        // POST: PhisicalLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phisicalLibrary = await _context.PhisicalLibraries.FindAsync(id);
            _context.PhisicalLibraries.Remove(phisicalLibrary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhisicalLibraryExists(int id)
        {
            return _context.PhisicalLibraries.Any(e => e.Id == id);
        }
    }
}
