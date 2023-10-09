using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    [Authorize] //é preciso login para aceder aos bookings
    public class BookingsController : Controller
    {
        private readonly DataContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookRepository _bookRepository;

        public BookingsController(DataContext context, IBookingRepository bookingRepository, IBookRepository bookRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _bookRepository = bookRepository;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            //sincronizar os bookings com os users
            var model = await _bookingRepository.GetBookingAsync(this.User.Identity.Name);
            return View(model);
        }

        
        //QD clicar no botão create -> aparece a view com a lista temprária de livros do user
        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            var model = await _bookingRepository.GetDetailTempAsync(this.User.Identity.Name);
            return View(model);
        }

        //Traz os items através do _bookRepository; e usa o modelo AddItemViewModel -> combobox
        public IActionResult AddBook()
        {
            var model = new AddItemViewModel
            {
                Books = _bookRepository.GetComboBooks()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddItemViewModel model)
        {
            //o book vai receber um modelo -> se for válido -> passo-lhe o modelo e o user
            if (ModelState.IsValid)
            {
                await _bookingRepository.AddItemToBookingAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

            //se correr alg coisa mal -> retorna a view e o user preenche outra vez
            return View(model);
        }


        public async Task <IActionResult> DeleteItem(int ? id)
        {
            if(id == null)
            {
                return new NotFoundViewResult("BookingNotFound");
            }

            await _bookingRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }


        //Após confirmar o booking -> redirecciona para o index
        public async Task<IActionResult> ConfirmBooking()
        {
            var response = await _bookingRepository.ConfirmBookingAsync(this.User.Identity.Name);
            if(response)
            {
                return RedirectToAction("Index");
            }

            //se n correr bem -> redireciona para o create
            return RedirectToAction("Create");
        }

        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Accept(int ? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookingNotFound");
            }

            var booking = await _bookingRepository.GetBookingAsync(id.Value);

            if(booking == null)
            {
                return new NotFoundViewResult("BookingNotFound");
            }

            DateTime today = DateTime.Now;
            DateTime expireDate = today.AddDays(5);

            var model = new FinishDateBookingViewModel
            {
                Id = booking.Id,
                LimitDate = expireDate,
            };

            return View(model);
        }


        public async Task<IActionResult> Accept(FinishDateBookingViewModel model)
        {
            if(ModelState.IsValid)
            {
                await _bookingRepository.AcceptBooking(model);
                return RedirectToAction("Index");
            }

            return View();
        }


        // GET: Bookings/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookingNotFound");
            }

            var booking = await _bookingRepository.GetBookingAsync(id.Value);
                
            if (booking == null)
            {
                return new NotFoundViewResult("BookingNotFound");
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ? id)
        {
            var booking = await _bookingRepository.GetBookingAsync(id.Value);

            try
            {
                await _bookingRepository.DeleteDetailTempAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException ex)
            {
                if(true)
                {
                    ViewBag.ErrorTitle = $"{booking.User}, maybe being used.";
                    ViewBag.ErrorMessage = $"{booking.User}, cannot be deleted until the booking is in role.</br></br>" +
                        $"Once the loan ends, try deleting it.";
                }

                return View("Error");
            }
        }


        public IActionResult BookingNotFound()
        {
            return View();
        }






        // GET: Bookings/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var booking = await _context.Bookings
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(booking);
        //}


        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,BookingDate,BookingStatus,BookingExpirationDate,LimitToGetTheBook")] Booking booking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(booking);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(booking);
        //}

        // GET: Bookings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var booking = await _context.Bookings.FindAsync(id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(booking);
        //}

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,BookingDate,BookingStatus,BookingExpirationDate,LimitToGetTheBook")] Booking booking)
        //{
        //    if (id != booking.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(booking);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookingExists(booking.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(booking);
        //}



        //private bool BookingExists(int id)
        //{
        //    return _context.Bookings.Any(e => e.Id == id);
        //}
    }
}
