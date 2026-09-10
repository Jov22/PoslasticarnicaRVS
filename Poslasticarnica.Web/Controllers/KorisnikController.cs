using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.Web.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Poslasticarnica.Web.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly HttpClient _httpClient;

        public KorisnikController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Prijava()
        {
            return View(
                new PrijavaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Prijava(
            PrijavaViewModel model)
        {
            if (string.IsNullOrWhiteSpace(
                    model.KorisnickoIme) ||
                string.IsNullOrWhiteSpace(
                    model.Lozinka))
            {
                ViewBag.Greska =
                    "Unesite korisničko ime i lozinku.";

                return View(model);
            }

            string url =
                "https://localhost:7016" +
                "/api/Korisnik/prijava" +
                $"?korisnickoIme={Uri.EscapeDataString(model.KorisnickoIme)}" +
                $"&lozinka={Uri.EscapeDataString(model.Lozinka)}";

            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(url);

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Pogrešno korisničko ime ili lozinka.";

                return View(model);
            }

            string json =
                await odgovor.Content.ReadAsStringAsync();

            Korisnik? korisnik =
                JsonSerializer.Deserialize<Korisnik>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (korisnik == null)
            {
                ViewBag.Greska =
                    "Greška pri prijavi.";

                return View(model);
            }

            HttpContext.Session.SetString(
                "KorisnickoIme",
                korisnik.KorisnickoIme);

            HttpContext.Session.SetString(
                "Uloga",
                korisnik.Uloga);

            return RedirectToAction(
                "Index",
                "Home");
        }

        [HttpGet]
        public IActionResult Registracija()
        {
            return View(
                new RegistracijaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Registracija(
            RegistracijaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Korisnik korisnik =
                new Korisnik
                {
                    KorisnickoIme =
                        model.KorisnickoIme,

                    Lozinka =
                        model.Lozinka,

                    Uloga =
                        "Korisnik"
                };

            HttpResponseMessage odgovor =
                await _httpClient.PostAsJsonAsync(
                    "https://localhost:7016" +
                    "/api/Korisnik/registracija",
                    korisnik);

            if (!odgovor.IsSuccessStatusCode)
            {
                string greska =
                    await odgovor.Content.ReadAsStringAsync();

                ViewBag.Greska =
                    string.IsNullOrWhiteSpace(greska)
                        ? "Registracija nije uspela."
                        : greska;

                return View(model);
            }

            TempData["Poruka"] =
                "Registracija je uspešna. Možete se prijaviti.";

            return RedirectToAction(
                "Prijava");
        }

        public IActionResult Odjava()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Prijava",
                "Korisnik");
        }
    }
}