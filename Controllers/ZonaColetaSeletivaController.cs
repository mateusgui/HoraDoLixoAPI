using HoraDoLixo.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace HoraDoLixo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonaColetaSeletivaController: ControllerBase
    {
        private readonly IZonaColetaSeletivaService _zona;
        public ZonaColetaSeletivaController(IZonaColetaSeletivaService service)
        {
            _zona = service;
        }

        [HttpGet] public IActionResult GetAll() => Ok(_zona.GetAll());
    }
}
