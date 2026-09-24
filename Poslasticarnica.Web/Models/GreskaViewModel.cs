namespace Poslasticarnica.Web.Models
{
    public class GreskaViewModel
    {
        public string? IdZahteva { get; set; }

        public bool PrikaziIdZahteva =>
            !string.IsNullOrEmpty(IdZahteva);
    }
}