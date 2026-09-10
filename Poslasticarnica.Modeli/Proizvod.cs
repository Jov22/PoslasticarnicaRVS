using System.ComponentModel.DataAnnotations;

namespace Poslasticarnica.Modeli
{
    public class Proizvod
    {
        public int ProizvodID { get; set; }

        [Required(ErrorMessage = "Naziv proizvoda je obavezan.")]
        [StringLength(
            100,
            ErrorMessage = "Naziv proizvoda može imati najviše 100 karaktera.")]
        public string Naziv { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategorija je obavezna.")]
        [StringLength(
            50,
            ErrorMessage = "Kategorija može imati najviše 50 karaktera.")]
        public string Kategorija { get; set; } = string.Empty;

        [Range(
            typeof(decimal),
            "1",
            "1000000",
            ErrorMessage = "Cena mora biti veća od 0.")]
        public decimal Cena { get; set; }
    }
}