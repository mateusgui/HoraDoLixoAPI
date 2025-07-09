using HoraDoLixo.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace HoraDoLixo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgamacaoColetaComumController: ControllerBase
    {
        private readonly IProgramacaoColetaComumService _coleta;
        public ProgamacaoColetaComumController(IProgramacaoColetaComumService service)
        {
            _coleta = service;
        }

        [HttpGet] public IActionResult GetAll() => Ok(_coleta.GetAll());
    }
}
