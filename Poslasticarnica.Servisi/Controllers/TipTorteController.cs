using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.Repozitorijum;

namespace Poslasticarnica.Servisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipTorteController : ControllerBase
    {
        [HttpGet]
        public IActionResult VratiSve()
        {
            TipTorteRepozitorijum repo =
                new TipTorteRepozitorijum();

            return Ok(repo.VratiSve());
        }

        [HttpGet("{id}")]
        public IActionResult VratiPoId(int id)
        {
            TipTorteRepozitorijum repo =
                new TipTorteRepozitorijum();

            TipTorte? tip =
                repo.VratiPoId(id);

            if (tip == null)
            {
                return NotFound();
            }

            return Ok(tip);
        }

        [HttpPost]
        public IActionResult Dodaj(TipTorte tip)
        {
            if (string.IsNullOrWhiteSpace(tip.Naziv))
            {
                return BadRequest(
                    "Naziv tipa torte je obavezan.");
            }

            TipTorteRepozitorijum repo =
                new TipTorteRepozitorijum();

            repo.Dodaj(tip);

            return Ok();
        }

        [HttpPut]
        public IActionResult Izmeni(TipTorte tip)
        {
            if (string.IsNullOrWhiteSpace(tip.Naziv))
            {
                return BadRequest(
                    "Naziv tipa torte je obavezan.");
            }

            TipTorteRepozitorijum repo =
                new TipTorteRepozitorijum();

            repo.Izmeni(tip);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Obrisi(int id)
        {
            TipTorteRepozitorijum repo =
                new TipTorteRepozitorijum();

            repo.Obrisi(id);

            return Ok();
        }
    }
}