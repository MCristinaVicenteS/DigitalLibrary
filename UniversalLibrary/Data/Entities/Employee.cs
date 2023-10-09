using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UniversalLibrary.Data.Entities
{
    public class Employee : IEntity
    {
        //herdar de um identityUser

        public int Id { get; set; }

        public User User { get; set; } //Tirar isto

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        //inserir um username

        public string ImageEmployee { get; set; }


        [Display(Name = "Nif")]
        public int Nif { get; set; } = 0!;

                
        [Display(Name = "Since")]
        public DateTime Since { get; set; }


        [Required]
        [Display(Name = "Have contract?")]
        public bool Contract { get; set; }


        [Required]
        [Display(Name = "Type of employee")]
        public string TypeOfEmployee { get; set; }


        public string EmployeeInformation
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
