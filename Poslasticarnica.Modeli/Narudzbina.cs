using System.ComponentModel.DataAnnotations;

namespace Poslasticarnica.Modeli
{
    public class Narudzbina
    {
        public int NarudzbinaID { get; set; }

        [Required(ErrorMessage = "Ime i prezime kupca je obavezno.")]
        [StringLength(
            100,
            ErrorMessage = "Ime i prezime može imati najviše 100 karaktera.")]
        public string ImePrezimeKupca { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "Broj telefona je obavezan.")]
        [RegularExpression(
            @"^06[0-9]{7,8}$",
            ErrorMessage = "Telefon mora biti u formatu 06xxxxxxxx ili 06xxxxxxx.")]
        public string BrojTelefona { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(
            ErrorMessage = "Email adresa nije ispravna.")]
        public string Email { get; set; }
            = string.Empty;

        public DateTime DatumKreiranja { get; set; }

        [Required(
            ErrorMessage = "Datum preuzimanja je obavezan.")]
        public DateTime DatumPreuzimanja { get; set; }

        public string Status { get; set; }
            = string.Empty;

       
        public decimal UkupnaCena { get; set; }

        [StringLength(
            500,
            ErrorMessage = "Napomena može imati najviše 500 karaktera.")]
        public string? Napomena { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Morate izabrati tip torte.")]
        public int TipTorteID { get; set; }
    }
}