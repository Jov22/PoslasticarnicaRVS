using Microsoft.AspNetCore.Mvc;
using Poslasticarnica.Repozitorijum;

namespace Poslasticarnica.Servisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProizvodController : ControllerBase
    {
        [HttpGet]
        public IActionResult VratiSve()
        {
            ProizvodRepozitorijum repo =
                new ProizvodRepozitorijum();

            return Ok(repo.VratiSve());
        }

        [HttpGet("{id}")]
        public IActionResult VratiPoId(int id)
        {
            ProizvodRepozitorijum repo =
                new ProizvodRepozitorijum();

            var proizvod =
                repo.VratiPoId(id);

            if (proizvod == null)
            {
                return NotFound();
            }

            return Ok(proizvod);
        }
    }
}