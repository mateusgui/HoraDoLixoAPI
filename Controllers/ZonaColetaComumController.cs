using HoraDoLixo.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace HoraDoLixo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonaColetaComumController : ControllerBase
    {
        private readonly IZonaColetaComumService _zonaColetaComumService;
        public ZonaColetaComumController(IZonaColetaComumService service)
        {
            _zonaColetaComumService = service;
        }

        [HttpGet] public IActionResult GetAll() => Ok(_zonaColetaComumService.GetAll());
    }
}
