using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Poslasticarnica.Servisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametarController : ControllerBase
    {
        [HttpGet]
        public IActionResult VratiParametre()
        {
            string putanja = Path.Combine(
                Directory.GetCurrentDirectory(),
                "parametri.json");

            string json = System.IO.File.ReadAllText(putanja);

            var parametri =
                JsonSerializer.Deserialize<Dictionary<string, int>>(json);

            return Ok(parametri);
        }
    }
}