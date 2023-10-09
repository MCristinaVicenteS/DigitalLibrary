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
    public class RecomendationsController : Controller
    {
        private readonly DataContext _context;

        public RecomendationsController(DataContext context)
        {
            _context = context;
        }

        // GET: Recomendations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recomendations.ToListAsync());
        }

        // GET: Recomendations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recomendation = await _context.Recomendations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recomendation == null)
            {
                return NotFound();
            }

            return View(recomendation);
        }

        // GET: Recomendations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recomendations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comments")] Recomendation recomendation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recomendation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recomendation);
        }

        // GET: Recomendations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recomendation = await _context.Recomendations.FindAsync(id);
            if (recomendation == null)
            {
                return NotFound();
            }
            return View(recomendation);
        }

        // POST: Recomendations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comments")] Recomendation recomendation)
        {
            if (id != recomendation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recomendation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecomendationExists(recomendation.Id))
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
            return View(recomendation);
        }

        // GET: Recomendations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recomendation = await _context.Recomendations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recomendation == null)
            {
                return NotFound();
            }

            return View(recomendation);
        }

        // POST: Recomendations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recomendation = await _context.Recomendations.FindAsync(id);
            _context.Recomendations.Remove(recomendation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecomendationExists(int id)
        {
            return _context.Recomendations.Any(e => e.Id == id);
        }
    }
}
