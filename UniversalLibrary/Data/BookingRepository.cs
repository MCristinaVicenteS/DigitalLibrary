using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;
using System;

namespace UniversalLibrary.Data
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public BookingRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Bookings.Include(p => p.User);
        }

        public async Task<List<Booking>> GetBookingAsync(string userName) //list pq n posso devolver iqueryable
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if(user == null)
            {
                return null;
            }

            //se existir user -> ver qual é o role para definir os acessos
            //role admin tem acesso a todos os Bookings
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //se for true -> vou buscar td os bookings à BD (tabela bookings)
                //incluir os items e os books q estão na tabela
                return await _context.Bookings
                    .Include(i => i.Items)              //tem ligação directa
                    .ThenInclude(b => b.Book)           //n tem ligação directa
                    .OrderByDescending(d => d.BookingDate)
                    .ToListAsync();
            }

            //qd associar os roles aos user -> criar um else if para os employess
            return await _context.Bookings
                .Include(i => i.Items)
                .ThenInclude(b => b.Book)
                .Where(u => u.User == user)
                .OrderByDescending (d => d.BookingDate)
                .ToListAsync();   
        }


        public async Task<IQueryable<BookingDetailTemp>> GetDetailTempAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if(user == null)
            {
                return null;
            }

            //se o user n for nulo -> vai buscar os dados temporários -> incluir os Books q reserva
            return _context.BookingDetailTemps
                .Include(b => b.Book)
                .Where(u => u.User == user)
                .OrderBy(o => o.Book.Title);
        }


        public async Task AddItemToBookingAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return;
            }

            //verificar se o book escolhido está na tabela books -> se sim -> trás o book c esse id
            var book = await _context.Books.FindAsync(model.BookId);
            if(book == null)
            {
                return;
            }

            //tendo user e book -> crio um objecto BookingDetailTemp
            //verifico se já há alg criado no _context
            var bookingDetailTemp = await _context.BookingDetailTemps
                .Where(u => u.User == user && u.Book == book)
                .FirstOrDefaultAsync();

            //se o bookingDetailTemp for nulo -> crio um novo
            if(bookingDetailTemp == null)
            {
                bookingDetailTemp = new BookingDetailTemp
                {
                    User = user,
                    Book = book,
                };

                _context.BookingDetailTemps.Add(bookingDetailTemp); //Adiciono aqui o objecto ao context
            }

            else
            {
                if(bookingDetailTemp.Book == model.Books) //se a lista já tiver este livro -> n adiciona
                {
                    _context.BookingDetailTemps.Remove(bookingDetailTemp);
                }
            }

            await _context.SaveChangesAsync(); //grava na BD
        }


        public async Task DeleteDetailTempAsync(int id)
        {
            var bookingDetailTemp = await _context.BookingDetailTemps.FindAsync(id);

            if(bookingDetailTemp == null)
            {
                return;
            }

            //se n for nulo -> vai ao _contex e remove o objecto bookingDetailTemp
            _context.BookingDetailTemps.Remove(bookingDetailTemp);

            await _context.SaveChangesAsync(); //grava na BD
        }


        //Neste método -> passo a tabela temporária para a tabela detail e no fim crio o booking
        public async Task<bool> ConfirmBookingAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return false;
            }

            //se o user n for nulo -> vai buscar a tabela temporária à BD
            //recebe td os books deste user e converte numa lista
            var bookingTemp = await _context.BookingDetailTemps
                .Include(b => b.Book)
                .Where(u => u.User == user)
                .ToListAsync();

            //verificar se a lista temporária é nula ou está vazia
            if(bookingTemp == null || bookingTemp.Count == 0)
            {
                return false;
            }

            //transferir esta lista para outra tabela
            //seleccion um book temporário um a um e converte para BookingDetail -> no final uma lista
            var details = bookingTemp.Select(l => new BookingDetail
            {
                Book = l.Book,
            }).ToList();

            //criar um objecto Booking
            var booking = new Booking
            {
                BookingDate = DateTime.UtcNow,   //hora do pc
                User = user,
                Items = details
            };

            //Remover da BD td os items da tabela temporária
            _context.BookingDetailTemps.RemoveRange(bookingTemp);
            await _context.SaveChangesAsync(true);

            //Actualizar a BD
            await CreateAsync(booking);
            return true;
        }


        //Aceitar o booking -> só o admin e o employee
        public async Task AcceptBooking(FinishDateBookingViewModel model)
        {
            var booking = await _context.Bookings.FindAsync(model.Id);
            if(booking == null)
            {
                return;
            }

            booking.BookingExpirationDate = (DateTime)model.LimitDate;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync(true);
        }

       

        // retorna uma lista de reservas booking
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        

        public async Task<Booking> GetBookingAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        // Seleciona a reserva do utilizador.
        public IEnumerable<SelectListItem> GetComboBookig()
        {
            var list = _context.Bookings
                .Select(p => new SelectListItem
                {
                    Text = p.User.FullName, 
                    Value = p.Id.ToString(),
                })
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Booking:",
                Value = "0"
            });

            return list;
        }

        
    }

}

