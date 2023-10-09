using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Models
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"\d{9}", ErrorMessage = "The field {0} only can contain 9 numbers lenght.")]
        //[Required]
        [Display(Name = "Nif")]
        public int Nif { get; set; }

        //[Required]
        [MaxLength(100, ErrorMessage = "The Field {0} only can contain {1} characters lenght")]
        public string Address { get; set; }


        [MaxLength(20, ErrorMessage = "The Field {0} only can contain {1} characters lenght")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

    }
}
