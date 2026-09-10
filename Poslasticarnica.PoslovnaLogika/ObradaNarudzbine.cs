using Poslasticarnica.Modeli;

namespace Poslasticarnica.PoslovnaLogika
{
    public class ObradaNarudzbine
    {
        public Narudzbina Obradi(
            Narudzbina narudzbina,
            int brojDanaZaHitno,
            int procenatDodatneNaknade)
        {
            DateTime granicniDatum =
                narudzbina.DatumKreiranja.AddDays(brojDanaZaHitno);

            if (narudzbina.DatumPreuzimanja <= granicniDatum)
            {
                narudzbina.Status = "Hitno";

                narudzbina.UkupnaCena =
                    narudzbina.UkupnaCena +
                    (narudzbina.UkupnaCena *
                    procenatDodatneNaknade / 100);
            }
            else
            {
                narudzbina.Status = "Standardno";
            }

            return narudzbina;
        }
    }
}
