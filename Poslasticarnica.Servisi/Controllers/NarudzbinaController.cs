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

            var narudzbina = repo.VratiPoId(id);

            if (narudzbina == null)
            {
                return NotFound();
            }

            return Ok(narudzbina);
        }

        [HttpPost]
        public IActionResult Dodaj(Narudzbina narudzbina)
        {
            narudzbina = ObradiNarudzbinu(narudzbina);

            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

            repo.Dodaj(narudzbina);

            return Ok(narudzbina);
        }

        [HttpPut]
        public IActionResult Izmeni(Narudzbina narudzbina)
        {
            // Pri izmeni se poslovno pravilo ponovo primenjuje
            // nad osnovnom cenom koja stiže iz Web forme.
            narudzbina = ObradiNarudzbinu(narudzbina);

            NarudzbinaRepozitorijum repo =
                new NarudzbinaRepozitorijum();

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

        private Narudzbina ObradiNarudzbinu(Narudzbina narudzbina)
        {
            string putanja = Path.Combine(
                Directory.GetCurrentDirectory(),
                "parametri.json");

            string json =
                System.IO.File.ReadAllText(putanja);

            var parametri =
                JsonSerializer.Deserialize<Dictionary<string, int>>(json);

            int brojDanaZaHitno =
                parametri["BrojDanaZaHitno"];

            int procenatDodatneNaknade =
                parametri["ProcenatDodatneNaknade"];

            ObradaNarudzbine obrada =
                new ObradaNarudzbine();

            return obrada.Obradi(
                narudzbina,
                brojDanaZaHitno,
                procenatDodatneNaknade);
        }
    }
}