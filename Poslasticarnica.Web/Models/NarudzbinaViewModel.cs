using Poslasticarnica.Modeli;

namespace Poslasticarnica.Web.Models
{
    public class NarudzbinaViewModel
    {
        public Narudzbina Narudzbina { get; set; }
            = new Narudzbina();

        public List<TipTorte> TipoviTorte { get; set; }
            = new List<TipTorte>();

        public List<StavkaNarudzbine> Stavke { get; set; }
            = new List<StavkaNarudzbine>();
    }
}