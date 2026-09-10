using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.Repozitorijum;

namespace Poslasticarnica.Servisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        [HttpGet("prijava")]
        public IActionResult Prijava(
            string korisnickoIme,
            string lozinka)
        {
            KorisnikRepozitorijum repo =
                new KorisnikRepozitorijum();

            Korisnik? korisnik =
                repo.Prijava(
                    korisnickoIme,
                    lozinka);

            if (korisnik == null)
            {
                return Unauthorized(
                    "Pogrešno korisničko ime ili lozinka.");
            }

            return Ok(korisnik);
        }

        [HttpPost("registracija")]
        public IActionResult Registracija(
            Korisnik korisnik)
        {
            if (string.IsNullOrWhiteSpace(
                    korisnik.KorisnickoIme))
            {
                return BadRequest(
                    "Korisničko ime je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(
                    korisnik.Lozinka))
            {
                return BadRequest(
                    "Lozinka je obavezna.");
            }

            KorisnikRepozitorijum repo =
                new KorisnikRepozitorijum();

            if (repo.PostojiKorisnickoIme(
                    korisnik.KorisnickoIme))
            {
                return BadRequest(
                    "Korisničko ime već postoji.");
            }

            korisnik.Uloga = "Korisnik";

            repo.Registruj(korisnik);

            return Ok();
        }
    }
}