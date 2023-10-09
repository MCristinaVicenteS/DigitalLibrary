using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class Booking : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "User Name")]
        [Required]
        public User User { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Booking Date")]
        [Required]
        public DateTime BookingDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Booking Expiration Date")]
        [Required]
        public DateTime BookingExpirationDate { get; set; }  //Data limite para levantar um livro reservado


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Booking Expiration Date")]
        [Required]
        public DateTime BookingDeliveredDate { get; set; } //Data de entrega da reserva


        [Display(Name = "Booking Status")]
        [Required]
        public string BookingStatus { get; set; } // Estado ex: Pending, delivered, closed, etc


        //Ligação c a tabela BookingDetail -> od estão os books -> entram por aqui os livros escolhidos na tabela temporária
        public IEnumerable<BookingDetail> Items { get; set; }


        //qt de encomendas
        public int QuantityOfBooking => Items == null ? 0 : Items.Count();


        //Qt de livros por encomenda 
        public int QuantityOfBooks => Items == null ? 0 : Items.Sum(i => i.QuantityOfBooks);

    }
}
