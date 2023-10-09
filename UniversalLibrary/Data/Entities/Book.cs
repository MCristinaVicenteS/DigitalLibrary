using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalLibrary.Data.Entities
{
    public class Book : IEntity
    {
        public int Id { get; set; }

        //NOTA: inserir o user que criou o livro
        public virtual User User { get; set; }

        public string UserId { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Title")]
        //[Required]
        public string Title { get; set; }

        public string Image { get; set; }

        [NotMapped]
        //[Required]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Author")]
        //[Required]        
        public virtual Author Author { get; set; }

        //[Required]
        public int AuthorId { get; set; }


        //adiconar a conecção Book com Author
        public ICollection<Author> Authors { get; set;}


        [Display(Name = "Phisical Library")]
        public virtual PhisicalLibrary PhisicalLibrary { get; set; }

        public int PhisicalId { get; set; }

        public ICollection<PhisicalLibrary> PhisicalLibraries { get; set; } //é aqui que as tabelas Book e PshisicalLibrary se ligam -> 1para mt


        [Display(Name = "Category")]
        //[Required]
        public virtual Category Category { get; set; }

        //[Required]
        public int CategoryId { get; set; }


        [Display(Name = "Available")]
        //[Required]
        public bool IsAvailable { get; set; }


        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        //[Required]
        public int Stock { get; set; }


        [Display(Name = "Book Publisher")]
        //[Required]
        public virtual BookPublisher BookPublisher { get; set; }

        //[Required]
        public int BookPublisherId { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Year")]
        //[Required]
        public DateTime Year { get; set; }


        [Display(Name = "Number of Pages")]
        public int PageNumber { get; set; }


        [Display(Name = "ISBN")]
        public string Isbn { get; set; }

    }
}
