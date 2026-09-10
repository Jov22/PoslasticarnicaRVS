using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.Web.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Poslasticarnica.Web.Controllers
{
    public class StavkaNarudzbineController : Controller
    {
        private readonly HttpClient _httpClient;

        public StavkaNarudzbineController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Dodaj(
            int narudzbinaID)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            StavkaNarudzbineViewModel model =
                new StavkaNarudzbineViewModel
                {
                    Stavka =
                        new StavkaNarudzbine
                        {
                            NarudzbinaID =
                                narudzbinaID,

                            Kolicina = 1
                        },

                    Proizvodi =
                        await VratiProizvode()
                };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Dodaj(
            StavkaNarudzbineViewModel model)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            if (!ModelState.IsValid)
            {
                model.Proizvodi =
                    await VratiProizvode();

                return View(model);
            }

            HttpResponseMessage odgovor =
                await _httpClient.PostAsJsonAsync(
                    "https://localhost:7016/api/StavkaNarudzbine",
                    model.Stavka);

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri dodavanju stavke.";

                model.Proizvodi =
                    await VratiProizvode();

                return View(model);
            }

            return RedirectToAction(
                "Detalji",
                "Narudzbina",
                new
                {
                    id = model.Stavka.NarudzbinaID
                });
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

            StavkaNarudzbine? stavka =
                await VratiPoId(id);

            if (stavka == null)
            {
                return NotFound();
            }

            StavkaNarudzbineViewModel model =
                new StavkaNarudzbineViewModel
                {
                    Stavka = stavka,
                    Proizvodi =
                        await VratiProizvode()
                };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Izmeni(
            StavkaNarudzbineViewModel model)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            if (!ModelState.IsValid)
            {
                model.Proizvodi =
                    await VratiProizvode();

                return View(model);
            }

            HttpResponseMessage odgovor =
                await _httpClient.PutAsJsonAsync(
                    "https://localhost:7016/api/StavkaNarudzbine",
                    model.Stavka);

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri izmeni stavke.";

                model.Proizvodi =
                    await VratiProizvode();

                return View(model);
            }

            return RedirectToAction(
                "Detalji",
                "Narudzbina",
                new
                {
                    id = model.Stavka.NarudzbinaID
                });
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

            StavkaNarudzbine? stavka =
                await VratiPoId(id);

            if (stavka == null)
            {
                return NotFound();
            }

            return View(stavka);
        }

        [HttpPost]
        [ActionName("Obrisi")]
        public async Task<IActionResult> ObrisiPotvrdi(
            int id)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            StavkaNarudzbine? stavka =
                await VratiPoId(id);

            if (stavka == null)
            {
                return NotFound();
            }

            int narudzbinaID =
                stavka.NarudzbinaID;

            HttpResponseMessage odgovor =
                await _httpClient.DeleteAsync(
                    $"https://localhost:7016/api/StavkaNarudzbine/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                ViewBag.Greska =
                    "Greška pri brisanju stavke.";

                return View(stavka);
            }

            return RedirectToAction(
                "Detalji",
                "Narudzbina",
                new
                {
                    id = narudzbinaID
                });
        }

        private async Task<List<Proizvod>>
            VratiProizvode()
        {
            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    "https://localhost:7016/api/Proizvod");

            if (!odgovor.IsSuccessStatusCode)
            {
                return new List<Proizvod>();
            }

            string json =
                await odgovor.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Proizvod>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
                ?? new List<Proizvod>();
        }

        private async Task<StavkaNarudzbine?>
            VratiPoId(int id)
        {
            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    $"https://localhost:7016/api/StavkaNarudzbine/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return null;
            }

            string json =
                await odgovor.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<StavkaNarudzbine>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        private bool NijePrijavljen()
        {
            return string.IsNullOrEmpty(
                HttpContext.Session.GetString(
                    "KorisnickoIme"));
        }
    }
}