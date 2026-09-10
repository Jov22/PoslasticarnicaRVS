using Poslasticarnica.Modeli;

namespace Poslasticarnica.Web.Models
{
    public class StavkaNarudzbineViewModel
    {
        public StavkaNarudzbine Stavka { get; set; }
            = new StavkaNarudzbine();

        public List<Proizvod> Proizvodi { get; set; }
            = new List<Proizvod>();
    }
}