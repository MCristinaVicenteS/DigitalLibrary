using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Data.Entities
{
    public enum InfractionType 
    {
        [Display(Name ="Damged Book is 20 Euros")]
        DamagedBook,

        [Display(Name = "Delay Book is 10 Euros")]
        Delay,

        [Display(Name = "Delay Book is 50 Euros")]
        NotReturn

    }
}
