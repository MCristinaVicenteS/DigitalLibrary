using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class BookPublisher : IEntity
    {
        public BookPublisher()
        {
            Books = new HashSet<Book>();
            BookOnlines = new HashSet<BookOnline>();
        }
        public int Id { get; set; }

        [Display(Name = "User Name")]
        public User User { get; set; }

        [Display(Name = "Publisher Name")]
        [Required]
        public string PublisherName { get; set; }

        [Display(Name = "Publisher Logo")]
        public string ImagLog { get; set; }
        public virtual IEnumerable<Book> Books { get; set; }
        public virtual IEnumerable<BookOnline> BookOnlines { get; set; }

    }
}
