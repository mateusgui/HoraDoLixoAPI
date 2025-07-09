using HoraDoLixo.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace HoraDoLixo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramacaoColetaSeletivaController : ControllerBase
    {
        private readonly IProgramacaoColetaSeletivaService _coleta;
        public ProgramacaoColetaSeletivaController(IProgramacaoColetaSeletivaService service)
        {
            _coleta = service;
        }

        [HttpGet] public IActionResult GetAll() => Ok(_coleta.GetAll());
    }
}
