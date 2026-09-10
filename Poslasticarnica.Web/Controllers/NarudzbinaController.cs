using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Modeli;
using Poslasticarnica.Web.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Poslasticarnica.Web.Controllers
{
    public class NarudzbinaController : Controller
    {
        private readonly HttpClient _httpClient;

        public NarudzbinaController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient =
                httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(
            string pretraga,
            string status)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            List<Narudzbina> lista =
                await VratiSveNarudzbine();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                lista = lista
                    .Where(n =>
                        n.ImePrezimeKupca.Contains(
                            pretraga,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                lista = lista
                    .Where(n =>
                        n.Status.Equals(
                            status,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Pretraga = pretraga;
            ViewBag.Status = status;

            return View(lista);
        }

        public async Task<IActionResult> Detalji(int id)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            Narudzbina? narudzbina =
                await VratiNarudzbinuPoId(id);

            if (narudzbina == null)
            {
                return NotFound();
            }

            NarudzbinaViewModel model =
                new NarudzbinaViewModel
                {
                    Narudzbina = narudzbina,

                    TipoviTorte =
                        await VratiTipoveTorte(),

                    Stavke =
                        await VratiStavkeZaNarudzbinu(id)
                };

            return View(model);
        }

        public async Task<IActionResult> Potvrda(int id)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            Narudzbina? narudzbina =
                await VratiNarudzbinuPoId(id);

            if (narudzbina == null)
            {
                return NotFound();
            }

            NarudzbinaViewModel model =
                new NarudzbinaViewModel
                {
                    Narudzbina = narudzbina,

                    TipoviTorte =
                        await VratiTipoveTorte(),

                    Stavke =
                        await VratiStavkeZaNarudzbinu(id)
                };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Dodaj()
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            NarudzbinaViewModel model =
                new NarudzbinaViewModel
                {
                    Narudzbina =
                        new Narudzbina
                        {
                            DatumKreiranja =
                                DateTime.Now,

                            DatumPreuzimanja =
                                DateTime.Now.AddDays(1),

                            UkupnaCena = 0
                        },

                    TipoviTorte =
                        await VratiTipoveTorte()
                };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Dodaj(
            NarudzbinaViewModel model)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            if (model.Narudzbina == null)
            {
                ViewBag.Greska =
                    "Podaci o narudžbini nisu ispravni.";

                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            if (model.Narudzbina.DatumPreuzimanja
                <= DateTime.Now)
            {
                ViewBag.Greska =
                    "Datum preuzimanja mora biti u budućnosti.";

                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            model.Narudzbina.DatumKreiranja =
                DateTime.Now;

            /*
             * Nova narudžbina nema cenu
             * dok joj se ne dodaju stavke.
             */
            model.Narudzbina.UkupnaCena = 0;

            HttpResponseMessage odgovor =
                await _httpClient.PostAsJsonAsync(
                    "https://localhost:7016/api/Narudzbina",
                    model.Narudzbina);

            if (!odgovor.IsSuccessStatusCode)
            {
                string detalji =
                    await odgovor.Content
                        .ReadAsStringAsync();

                ViewBag.Greska =
                    $"Greška servisa: " +
                    $"{(int)odgovor.StatusCode} - " +
                    $"{detalji}";

                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
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

            Narudzbina? narudzbina =
                await VratiNarudzbinuPoId(id);

            if (narudzbina == null)
            {
                return NotFound();
            }

            NarudzbinaViewModel model =
                new NarudzbinaViewModel
                {
                    Narudzbina = narudzbina,

                    TipoviTorte =
                        await VratiTipoveTorte(),

                    Stavke =
                        await VratiStavkeZaNarudzbinu(id)
                };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Izmeni(
            NarudzbinaViewModel model)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            if (model.Narudzbina == null)
            {
                ViewBag.Greska =
                    "Podaci o narudžbini nisu ispravni.";

                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            if (model.Narudzbina.DatumPreuzimanja
                <= DateTime.Now)
            {
                ViewBag.Greska =
                    "Datum preuzimanja mora biti u budućnosti.";

                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            /*
             * Čuvamo trenutnu cenu iz baze.
             * Korisnik je ne menja ručno.
             */
            Narudzbina? postojeca =
                await VratiNarudzbinuPoId(
                    model.Narudzbina.NarudzbinaID);

            if (postojeca == null)
            {
                return NotFound();
            }

            model.Narudzbina.UkupnaCena =
                postojeca.UkupnaCena;

            HttpResponseMessage odgovor =
                await _httpClient.PutAsJsonAsync(
                    "https://localhost:7016/api/Narudzbina",
                    model.Narudzbina);

            if (!odgovor.IsSuccessStatusCode)
            {
                string detalji =
                    await odgovor.Content
                        .ReadAsStringAsync();

                ViewBag.Greska =
                    $"Greška servisa: " +
                    $"{(int)odgovor.StatusCode} - " +
                    $"{detalji}";

                model.TipoviTorte =
                    await VratiTipoveTorte();

                return View(model);
            }

            /*
             * Posle izmene datuma narudžbine
             * treba ponovo primeniti obračun,
             * jer status Hitno/Standardno
             * možda više nije isti.
             */
            await OsveziCenuPrekoStavki(
                model.Narudzbina.NarudzbinaID);

            return RedirectToAction(
                "Detalji",
                new
                {
                    id =
                        model.Narudzbina.NarudzbinaID
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

            Narudzbina? narudzbina =
                await VratiNarudzbinuPoId(id);

            if (narudzbina == null)
            {
                return NotFound();
            }

            return View(narudzbina);
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

            HttpResponseMessage odgovor =
                await _httpClient.DeleteAsync(
                    $"https://localhost:7016/api/Narudzbina/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return BadRequest(
                    "Greška pri brisanju narudžbine.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Stampa()
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            List<Narudzbina> lista =
                await VratiSveNarudzbine();

            return View(lista);
        }

        public async Task<IActionResult> StampaFiltrirano(
            string pretraga,
            string status)
        {
            if (NijePrijavljen())
            {
                return RedirectToAction(
                    "Prijava",
                    "Korisnik");
            }

            List<Narudzbina> lista =
                await VratiSveNarudzbine();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                lista = lista
                    .Where(n =>
                        n.ImePrezimeKupca.Contains(
                            pretraga,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                lista = lista
                    .Where(n =>
                        n.Status.Equals(
                            status,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(
                "Stampa",
                lista);
        }

        private async Task<List<Narudzbina>>
            VratiSveNarudzbine()
        {
            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    "https://localhost:7016/api/Narudzbina");

            if (!odgovor.IsSuccessStatusCode)
            {
                return new List<Narudzbina>();
            }

            string json =
                await odgovor.Content
                    .ReadAsStringAsync();

            return JsonSerializer
                .Deserialize<List<Narudzbina>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                ?? new List<Narudzbina>();
        }

        private async Task<Narudzbina?>
            VratiNarudzbinuPoId(int id)
        {
            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    $"https://localhost:7016/api/Narudzbina/{id}");

            if (!odgovor.IsSuccessStatusCode)
            {
                return null;
            }

            string json =
                await odgovor.Content
                    .ReadAsStringAsync();

            return JsonSerializer
                .Deserialize<Narudzbina>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
        }

        private async Task<List<TipTorte>>
            VratiTipoveTorte()
        {
            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    "https://localhost:7016/api/TipTorte");

            if (!odgovor.IsSuccessStatusCode)
            {
                return new List<TipTorte>();
            }

            string json =
                await odgovor.Content
                    .ReadAsStringAsync();

            return JsonSerializer
                .Deserialize<List<TipTorte>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                ?? new List<TipTorte>();
        }

        private async Task<List<StavkaNarudzbine>>
            VratiStavkeZaNarudzbinu(
                int narudzbinaID)
        {
            HttpResponseMessage odgovor =
                await _httpClient.GetAsync(
                    "https://localhost:7016" +
                    "/api/StavkaNarudzbine/narudzbina/" +
                    narudzbinaID);

            if (!odgovor.IsSuccessStatusCode)
            {
                return new List<StavkaNarudzbine>();
            }

            string json =
                await odgovor.Content
                    .ReadAsStringAsync();

            return JsonSerializer
                .Deserialize<List<StavkaNarudzbine>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                ?? new List<StavkaNarudzbine>();
        }

        /*
         * Trik koji koristimo da servis ponovo
         * preračuna cenu nakon izmene datuma.
         *
         * Ako narudžbina ima stavke, pošaljemo PUT
         * za prvu stavku bez promene podataka.
         * StavkaNarudzbine servis nakon PUT-a
         * automatski računa zbir i poslovno pravilo.
         */
        private async Task OsveziCenuPrekoStavki(
            int narudzbinaID)
        {
            List<StavkaNarudzbine> stavke =
                await VratiStavkeZaNarudzbinu(
                    narudzbinaID);

            if (stavke.Count == 0)
            {
                return;
            }

            StavkaNarudzbine prvaStavka =
                stavke[0];

            await _httpClient.PutAsJsonAsync(
                "https://localhost:7016/api/StavkaNarudzbine",
                prvaStavka);
        }

        private bool NijePrijavljen()
        {
            return string.IsNullOrEmpty(
                HttpContext.Session.GetString(
                    "KorisnickoIme"));
        }
    }
}