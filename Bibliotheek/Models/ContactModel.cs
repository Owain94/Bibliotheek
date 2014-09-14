#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Bibliotheek.Models
{
    public class ContactModel
    {
        [Display(Name = "Naam: ")]
        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }

        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Email is verplicht")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Onderwerp: ")]
        [Required(ErrorMessage = "Onderwerp is verplicht")]
        public string Subject { get; set; }

        [Display(Name = "Bericht: ")]
        [Required(ErrorMessage = "Bericht is verplicht")]
        public string Message { get; set; }
    }
}