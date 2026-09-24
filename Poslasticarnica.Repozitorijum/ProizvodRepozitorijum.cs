using Poslasticarnica.Modeli;

namespace Poslasticarnica.Repozitorijum
{
    public class ProizvodRepozitorijum
    {
        public List<Proizvod> VratiSve()
        {
            using (KontekstBaze kontekst = new KontekstBaze())
            {
                return kontekst.Proizvodi.ToList();
            }
        }

        public Proizvod? VratiPoId(int id)
        {
            using (KontekstBaze kontekst = new KontekstBaze())
            {
                return kontekst.Proizvodi
                    .FirstOrDefault(p => p.ProizvodID == id);
            }
        }

        public void Dodaj(Proizvod proizvod)
        {
            using (KontekstBaze kontekst = new KontekstBaze())
            {
                kontekst.Proizvodi.Add(proizvod);
                kontekst.SaveChanges();
            }
        }

        public void Izmeni(Proizvod proizvod)
        {
            using (KontekstBaze kontekst = new KontekstBaze())
            {
                Proizvod? postojeciProizvod =
                    kontekst.Proizvodi
                        .FirstOrDefault(
                            p => p.ProizvodID == proizvod.ProizvodID);

                if (postojeciProizvod != null)
                {
                    postojeciProizvod.Naziv = proizvod.Naziv;
                    postojeciProizvod.Kategorija = proizvod.Kategorija;
                    postojeciProizvod.Cena = proizvod.Cena;

                    kontekst.SaveChanges();
                }
            }
        }

        public void Obrisi(int id)
        {
            using (KontekstBaze kontekst = new KontekstBaze())
            {
                Proizvod? proizvod =
                    kontekst.Proizvodi
                        .FirstOrDefault(p => p.ProizvodID == id);

                if (proizvod != null)
                {
                    kontekst.Proizvodi.Remove(proizvod);
                    kontekst.SaveChanges();
                }
            }
        }
    }
}