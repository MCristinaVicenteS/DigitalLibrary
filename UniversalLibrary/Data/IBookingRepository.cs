using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Models;

namespace UniversalLibrary.Data
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        public IQueryable GetAllWithUsers();

        //task para ir buscar os booking associadas a um user
        Task<List<Booking>> GetBookingAsync(string userName);

        Task<Booking> GetBookingAsync(int id);

        //task q devolve os dados temporários do booking -> associa o booking ao user -> BookingDetailTemp
        Task<IQueryable<BookingDetailTemp>> GetDetailTempAsync(string userName);

        //task para adicionar os items à lista temporária
        Task AddItemToBookingAsync(AddItemViewModel model, string userName);

        //Task para apagar o item da lista temporária
        Task DeleteDetailTempAsync(int id);

        //task para confirmar o booking -> os dados passam para a lista definitiva
        //ou seja, passam da view create para a view index do controlador Booking
        //e a tabela temporária desaparece
        Task<bool> ConfirmBookingAsync(string userName);

        //task para aceitar o booking e dar uma data limite de levantamento do livro
        Task AcceptBooking(FinishDateBookingViewModel model);            

    }
}
