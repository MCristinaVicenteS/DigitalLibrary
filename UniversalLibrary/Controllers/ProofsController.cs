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
    public class ProofsController : Controller
    {
        private readonly DataContext _context;

        public ProofsController(DataContext context)
        {
            _context = context;
        }

        // GET: Proofs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proofs.ToListAsync());
        }

        // GET: Proofs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proof = await _context.Proofs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proof == null)
            {
                return NotFound();
            }

            return View(proof);
        }

        // GET: Proofs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proofs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,EmailNotification,PdfFileDocument,CreationDate")] Proof proof)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proof);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proof);
        }

        // GET: Proofs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proof = await _context.Proofs.FindAsync(id);
            if (proof == null)
            {
                return NotFound();
            }
            return View(proof);
        }

        // POST: Proofs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,EmailNotification,PdfFileDocument,CreationDate")] Proof proof)
        {
            if (id != proof.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proof);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProofExists(proof.Id))
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
            return View(proof);
        }

        // GET: Proofs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proof = await _context.Proofs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proof == null)
            {
                return NotFound();
            }

            return View(proof);
        }

        // POST: Proofs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proof = await _context.Proofs.FindAsync(id);
            _context.Proofs.Remove(proof);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProofExists(int id)
        {
            return _context.Proofs.Any(e => e.Id == id);
        }
    }
}
