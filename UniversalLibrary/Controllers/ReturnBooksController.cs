using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class ReturnBooksController : Controller
    {
        private readonly DataContext _context;
        private readonly IReturnBookRepository _returnBookRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public ReturnBooksController(DataContext context, IReturnBookRepository returnBookRepository, IUserHelper userHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _returnBookRepository = returnBookRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: ReturnBooks
        public async Task<IActionResult> Index()
        {
            return View(_returnBookRepository.GetAll().OrderBy(p => p.Id));
        }


        // GET: ReturnBooks/Create
        public IActionResult Create()
        {
            var model = new ReturnBookViewModel();
            //model.AvailableReaders = _returnBookRepository.GetComboReaders();
            //model.AvailableBooks = _returnBookRepository.GetComboBooks();

            return View(model);
        }

        // POST: ReturnBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReturnBookViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(returnBook);

            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            return View(model);
        }


        // GET: ReturnBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnBook = await _context.ReturnBooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (returnBook == null)
            {
                return NotFound();
            }

            return View(returnBook);
        }

        

        // GET: ReturnBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnBook = await _context.ReturnBooks.FindAsync(id);
            if (returnBook == null)
            {
                return NotFound();
            }
            return View(returnBook);
        }

        // POST: ReturnBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReturnDate,IsDamaged,PenaltyConfirmation,DamageDescription")] ReturnBook returnBook)
        {
            if (id != returnBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(returnBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReturnBookExists(returnBook.Id))
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
            return View(returnBook);
        }

        // GET: ReturnBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnBook = await _context.ReturnBooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (returnBook == null)
            {
                return NotFound();
            }

            return View(returnBook);
        }

        // POST: ReturnBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var returnBook = await _context.ReturnBooks.FindAsync(id);
            _context.ReturnBooks.Remove(returnBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReturnBookExists(int id)
        {
            return _context.ReturnBooks.Any(e => e.Id == id);
        }
    }
}
