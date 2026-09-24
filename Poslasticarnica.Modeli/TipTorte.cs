namespace Poslasticarnica.Modeli
{
    public class TipTorte
    {
        public int TipTorteID { get; set; }

        public string Naziv { get; set; } = string.Empty;

        public decimal Cena { get; set; }
    }
}