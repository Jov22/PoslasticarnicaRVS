using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.PoslovnaLogika;
using Poslasticarnica.Repozitorijum;
using System.Text.Json;

namespace Poslasticarnica.Servisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StavkaNarudzbineController : ControllerBase
    {
        [HttpGet("narudzbina/{narudzbinaID}")]
        public IActionResult VratiZaNarudzbinu(
            int narudzbinaID)
        {
            StavkaNarudzbineRepozitorijum repo =
                new StavkaNarudzbineRepozitorijum();

            return Ok(
                repo.VratiZaNarudzbinu(
                    narudzbinaID));
        }

        [HttpGet("{id}")]
        public IActionResult VratiPoId(int id)
        {
            StavkaNarudzbineRepozitorijum repo =
                new StavkaNarudzbineRepozitorijum();

            StavkaNarudzbine? stavka =
                repo.VratiPoId(id);

            if (stavka == null)
            {
                return NotFound();
            }

            return Ok(stavka);
        }

        [HttpPost]
        public IActionResult Dodaj(
            StavkaNarudzbine stavka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StavkaNarudzbineRepozitorijum repo =
                new StavkaNarudzbineRepozitorijum();

            repo.Dodaj(stavka);

            AzurirajCenuNarudzbine(
                stavka.NarudzbinaID);

            return Ok(stavka);
        }

        [HttpPut]
        public IActionResult Izmeni(
            StavkaNarudzbine stavka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StavkaNarudzbineRepozitorijum repo =
                new StavkaNarudzbineRepozitorijum();

            repo.Izmeni(stavka);

            AzurirajCenuNarudzbine(
                stavka.NarudzbinaID);

            return Ok(stavka);
        }

        [HttpDelete("{id}")]
        public IActionResult Obrisi(int id)
        {
            StavkaNarudzbineRepozitorijum repo =
                new StavkaNarudzbineRepozitorijum();

            StavkaNarudzbine? stavka =
                repo.VratiPoId(id);

            if (stavka == null)
            {
                return NotFound();
            }

            int narudzbinaID =
                stavka.NarudzbinaID;

            repo.Obrisi(id);

            AzurirajCenuNarudzbine(
                narudzbinaID);

            return Ok();
        }

        private void AzurirajCenuNarudzbine(
            int narudzbinaID)
        {
            NarudzbinaRepozitorijum narudzbinaRepo =
                new NarudzbinaRepozitorijum();

            Narudzbina? narudzbina =
                narudzbinaRepo.VratiPoId(
                    narudzbinaID);

            if (narudzbina == null)
            {
                return;
            }

            StavkaNarudzbineRepozitorijum stavkaRepo =
                new StavkaNarudzbineRepozitorijum();

            // Osnovna cena torte i svih dodataka,
            // bez naknade za hitnu narudzbinu.
            decimal osnovnaCena =
                stavkaRepo.IzracunajCenuTorteIDodataka(
                    narudzbinaID);

            // Uvek krecemo od ciste cene kako se
            // naknada za hitnost ne bi dodala vise puta.
            narudzbina.UkupnaCena =
                osnovnaCena;

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

            narudzbina =
                obrada.Obradi(
                    narudzbina,
                    brojDanaZaHitno,
                    procenatDodatneNaknade);

            narudzbinaRepo.Izmeni(
                narudzbina);
        }
    }
}