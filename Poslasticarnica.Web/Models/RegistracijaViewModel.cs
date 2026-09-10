using System.ComponentModel.DataAnnotations;

namespace Poslasticarnica.Web.Models
{
    public class RegistracijaViewModel
    {
        [Required(
            ErrorMessage =
                "Korisničko ime je obavezno.")]
        [StringLength(
            50,
            MinimumLength = 3,
            ErrorMessage =
                "Korisničko ime mora imati najmanje 3 karaktera.")]
        public string KorisnickoIme { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Lozinka je obavezna.")]
        [StringLength(
            50,
            MinimumLength = 4,
            ErrorMessage =
                "Lozinka mora imati najmanje 4 karaktera.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }
            = string.Empty;

        [Required(
            ErrorMessage =
                "Potvrda lozinke je obavezna.")]
        [DataType(DataType.Password)]
        [Compare(
            "Lozinka",
            ErrorMessage =
                "Lozinke se ne poklapaju.")]
        public string PotvrdaLozinke { get; set; }
            = string.Empty;
    }
}