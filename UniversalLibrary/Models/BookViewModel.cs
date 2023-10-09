using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Models
{
    public class BookViewModel
    {    
        public int Id { get; set; }

        public Book Book { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }


        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }


        //Propriedade para fazer o upload da imagem
        [Display(Name = "Book Cover")]
        public IFormFile ImageFile { get; set; }


        public string Image { get; set; }

      
        public IEnumerable<SelectListItem> AvailableAuthors { get; set; }

        // ID do autor selecionado
        [Display(Name = "Author")]
        [Required]
        public int AuthorId { get; set; }

        public Author Author { get; set; }
        
        
        public IEnumerable<SelectListItem> AvailableCategories { get; set; }             

        // ID da categoria selecionada
        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }
   
        public Category Category { get; set; }


        public IEnumerable<SelectListItem> AvailablePublishers { get; set; }

        // ID da editora selecionada
        [Display(Name = "Book Publisher")]
        [Required]
        public int BookPublisherId { get; set; }

        public BookPublisher BookPublisher { get; set; }      
        

        public IEnumerable<SelectListItem> AvailableLibrary { get; set; }

        [Display(Name = "Phisical Library")]
        [Required]
        public int PhisicalId { get; set; }

        public PhisicalLibrary PhisicalLibrary { get; set; }


        [Display(Name = "Available")]
        [Required]
        public bool IsAvailable { get; set; }


        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Required]
        public int Stock { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Year")]
        [Required]
        public DateTime Year { get; set; }


        [Display(Name = "Number of Pages")]
        public int PageNumber { get; set; }


        [Display(Name = "ISBN")]
        [Required]
        public string Isbn { get; set; }

    }
}
