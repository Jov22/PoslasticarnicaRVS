using System.ComponentModel.DataAnnotations;

namespace Poslasticarnica.Modeli
{
    public class StavkaNarudzbine
    {
        public int StavkaNarudzbineID { get; set; }

        public int NarudzbinaID { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Morate izabrati proizvod.")]
        public int ProizvodID { get; set; }

        [Range(
            1,
            1000,
            ErrorMessage = "Količina mora biti veća od 0.")]
        public int Kolicina { get; set; }

        public string NazivProizvoda { get; set; }
            = string.Empty;

        public decimal CenaPoKomadu { get; set; }

        public decimal UkupnaCenaStavke
        {
            get
            {
                return Kolicina * CenaPoKomadu;
            }
        }
    }
}