using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class BookOnline : IEntity
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Title")]        
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Title { get; set; }


        [Display(Name = "Image")]     
        public string Image { get; set; }


        [NotMapped]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Author")]
        public virtual Author Author { get; set; }      

        public int AuthorId { get; set; }


        //adiconar a conecção BookOnline com Author
        public ICollection<Author> Authors { get; set; }


        [Display(Name = "Category")]
        public virtual Category Category { get; set; }


        public int CategoryId { get; set; }


        [Display(Name = "Book Publisher")]
        public virtual BookPublisher BookPublisher { get; set; }


        public int BookPublisherId { get; set; }


        [Display(Name = "Year")]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Year { get; set; }


        [Display(Name = "Number of Pages")]
        public int PageNumber { get; set; }


        [Display(Name = "ISBN")]
        public string Isbn { get; set; }


        [Display(Name = "Book")]
        public string Pdf { get; set; }


        [NotMapped]
        public IFormFile PdfFile { get; set; }


        //[Display(Name = "Book Information")]
        //public string BookInformation => $"{Title}; Author - {Author.FirstName} {Author.LastName}";


        //Propriedade só de leitura c o caminho absoluto para o servidor
        //Info q vai aparecer na API c o local da imagem
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return null;
                }
                //ATENÇÃO -> ISTO ESTÁ COM O MEU LOCALHOST -> VEJAM SE DÁ NO VOSSO E DP ALTERAMOS PARA O AZUR
                return $"https://localhost:44319{Image.Substring(1)}";
            }
        }

        public string FileFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Pdf))
                {
                    return null;
                }
                //ATENÇÃO -> ISTO ESTÁ COM O MEU LOCALHOST -> VEJAM SE DÁ NO VOSSO E DP ALTERAMOS PARA O AZUR
                return $"https://localhost:44319{Pdf.Substring(1)}";
            }
        }
    }
}
