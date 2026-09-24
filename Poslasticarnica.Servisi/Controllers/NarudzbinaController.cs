using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.PoslovnaLogika;
using Poslasticarnica.Repozitorijum;
using System.Text.Json;

namespace Poslasticarnica.Servisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NarudzbinaController : ControllerBase
    {
        [HttpGet]
        public IActionResult VratiSve()
        {
            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

            return Ok(repo.VratiSve());
        }

        [HttpGet("{id}")]
        public IActionResult VratiPoId(int id)
        {
            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

            Narudzbina? narudzbina =
                repo.VratiPoId(id);

            if (narudzbina == null)
            {
                return NotFound();
            }

            return Ok(narudzbina);
        }

        [HttpPost]
        public IActionResult Dodaj(
            Narudzbina narudzbina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TipTorteRepozitorijum tipRepo =
                new TipTorteRepozitorijum();

            TipTorte? tipTorte =
                tipRepo.VratiPoId(
                    narudzbina.TipTorteID);

            if (tipTorte == null)
            {
                return BadRequest(
                    "Izabrani tip torte ne postoji.");
            }

            // Nova narudzbina prvo dobija
            // osnovnu cenu izabranog tipa torte.
            narudzbina.UkupnaCena =
                tipTorte.Cena;

            // Tek zatim se primenjuje poslovno pravilo
            // za hitnu narudzbinu.
            narudzbina =
                ObradiNarudzbinu(
                    narudzbina);

            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

            repo.Dodaj(narudzbina);

            return Ok(narudzbina);
        }

        [HttpPut]
        public IActionResult Izmeni(
            Narudzbina narudzbina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

            Narudzbina? postojecaNarudzbina =
                repo.VratiPoId(
                    narudzbina.NarudzbinaID);

            if (postojecaNarudzbina == null)
            {
                return NotFound();
            }

            TipTorteRepozitorijum tipRepo =
                new TipTorteRepozitorijum();

            TipTorte? tipTorte =
                tipRepo.VratiPoId(
                    narudzbina.TipTorteID);

            if (tipTorte == null)
            {
                return BadRequest(
                    "Izabrani tip torte ne postoji.");
            }

            StavkaNarudzbineRepozitorijum stavkaRepo =
                new StavkaNarudzbineRepozitorijum();

            decimal cenaDodataka =
                stavkaRepo.IzracunajOsnovnuCenu(
                    narudzbina.NarudzbinaID);

            // Pri izmeni ponovo racunamo cenu
            // od nule:
            // cena torte + svi dodaci.
            narudzbina.UkupnaCena =
                tipTorte.Cena +
                cenaDodataka;

            narudzbina =
                ObradiNarudzbinu(
                    narudzbina);

            repo.Izmeni(narudzbina);

            return Ok(narudzbina);
        }

        [HttpDelete("{id}")]
        public IActionResult Obrisi(int id)
        {
            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

            repo.Obrisi(id);

            return Ok();
        }

        private Narudzbina ObradiNarudzbinu(
            Narudzbina narudzbina)
        {
            string putanja =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "parametri.json");

            string json =
                System.IO.File.ReadAllText(
                    putanja);

            Dictionary<string, int>? parametri =
                JsonSerializer.Deserialize<
                    Dictionary<string, int>>(
                    json);

            int brojDanaZaHitno =
                parametri?["BrojDanaZaHitno"] ?? 3;

            int procenatDodatneNaknade =
                parametri?["ProcenatDodatneNaknade"] ?? 20;

            ObradaNarudzbine obrada =
                new ObradaNarudzbine();

            return obrada.Obradi(
                narudzbina,
                brojDanaZaHitno,
                procenatDodatneNaknade);
        }
    }
}