using System;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public class PhisicalLibrary : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Library Name")]
        [Required]
        public string LibraryName { get; set; }

        //public string Services { get; set; } // Passar para views

        //public string Topics { get; set; }

        //public string Events { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Foundation Date")]
        [Required]
        public DateTime FoundationDate { get; set; }


        [Display(Name = "Director's Name")]
        [Required]
        public string Director { get; set; }


        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Required]
        public int Stock { get; set; }

        [RegularExpression(@"\d{9}", ErrorMessage = "The field {0} need 9 numbers lenght.")]
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }


        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }



        [Display(Name = "Website")]
        [Required]
        [RegularExpression(@"^(https?://)?(www\.)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,}(\/\S*)?$", ErrorMessage = "Please enter a valid website.")]
        public string WebSite { get; set; }


        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = false)]
        [Display(Name = "Open Time")]
        [Required]
        public DateTime OpenTime { get; set; }


        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = false)]
        [Display(Name = "Close Time")]
        [Required]
        public DateTime CloseTime { get; set; }


        [Display(Name = "WorkingDay")]
        [Required]
        public string WorkingDay { get; set; }


        [Display(Name = "Google Map")]
        [Required]
        public string GoogleMap { get; set; }
    }
}
