using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Web.Models;

namespace Poslasticarnica.Web.Controllers
{
    public class PocetnaController : Controller
    {
        private readonly ILogger<PocetnaController> _logger;

        public PocetnaController(ILogger<PocetnaController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Greska()
        {
            return View(new GreskaViewModel
            {
                IdZahteva = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}