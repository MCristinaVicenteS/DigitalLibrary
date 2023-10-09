using System.ComponentModel.DataAnnotations;
using System;

namespace UniversalLibrary.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the start date.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the end date.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a book.")]
        public int BookId { get; set; }
    }
}
