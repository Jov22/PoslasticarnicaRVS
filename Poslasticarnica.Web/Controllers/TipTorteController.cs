using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using System.Net.Http.Json;
using System.Text.Json;

namespace Poslasticarnica.Web.Controllers
{
    public class TipTorteController : Controller
    {
        private readonly HttpClient _httpClient;

        public TipTorteController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    "https://localhost:7016/api/TipTorte");

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri učitavanju tipova torti.";

                return View(new List<TipTorte>());
            }

            string json =
                await odgovor.Content.ReadAsStringAsync();

            List<TipTorte> lista =
                JsonSerializer.Deserialize<List<TipTorte>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                ?? new List<TipTorte>();

            return View(lista);
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            return View(new TipTorte());
        }

        [HttpPost]
        public async Task<IActionResult> Dodaj(TipTorte tip)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            if (string.IsNullOrWhiteSpace(tip.Naziv))
            {
                ViewBag.Greska =
                    "Naziv tipa torte je obavezan.";

                return View(tip);
            }

            HttpResponseMessage odgovor =
                await _httpClient.PostAsJsonAsync(
                    "https://localhost:7016/api/TipTorte",
                    tip);

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri dodavanju tipa torte.";

                return View(tip);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Izmeni(int id)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    $"https://localhost:7016/api/TipTorte/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string json =
                await odgovor.Content.ReadAsStringAsync();

            TipTorte? tip =
                JsonSerializer.Deserialize<TipTorte>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        [HttpPost]
        public async Task<IActionResult> Izmeni(TipTorte tip)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            if (string.IsNullOrWhiteSpace(tip.Naziv))
            {
                ViewBag.Greska =
                    "Naziv tipa torte je obavezan.";

                return View(tip);
            }

            HttpResponseMessage odgovor =
                await _httpClient.PutAsJsonAsync(
                    "https://localhost:7016/api/TipTorte",
                    tip);

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri izmeni tipa torte.";

                return View(tip);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Obrisi(int id)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    $"https://localhost:7016/api/TipTorte/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return NotFound();
            }

            string json =
                await odgovor.Content.ReadAsStringAsync();

            TipTorte? tip =
                JsonSerializer.Deserialize<TipTorte>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        [HttpPost]
        [ActionName("Obrisi")]
        public async Task<IActionResult> ObrisiPotvrdi(int id)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            HttpResponseMessage odgovor =
                await _httpClient.DeleteAsync(
                    $"https://localhost:7016/api/TipTorte/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return BadRequest(
                    "Tip torte nije moguće obrisati.");
            }

            return RedirectToAction("Index");
        }

        private bool NijePrijavljen()
        {
            return string.IsNullOrEmpty(
                HttpContext.Session.GetString("KorisnickoIme"));
        }
    }
}