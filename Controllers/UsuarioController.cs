using HoraDoLixo.Dto;
using HoraDoLixo.Model;
using HoraDoLixo.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HoraDoLixo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuario;
        public UsuarioController(IUsuarioService service)
        {
            _usuario = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = _usuario.GetAll();

            foreach (var usuario in usuarios)
            {
                usuario.SenhaHash = "********";
            }

            return Ok(usuarios);
        }

        [HttpGet("{id:int}", Name = "GetUsuarioById")]
        public IActionResult GetById(int id)
        {
            var usuario = _usuario.GetById(id);
            if (usuario == null) return NotFound();
            usuario.SenhaHash = "********";
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDto usuarioDto)
        {
            try
            {
                var novoUsuario = await _usuario.CreateAsync(usuarioDto);
                if (novoUsuario == null || novoUsuario.IdUsuario == 0)
                {
                    return StatusCode(500, "Falha ao criar o usuário. Id não gerado.");
                }
                novoUsuario.SenhaHash = "********";
                return CreatedAtRoute(
                    routeName: "GetUsuarioById",
                    routeValues: new { id = novoUsuario.IdUsuario },
                    value: novoUsuario);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao processar o cadastro.");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto usuarioDto)
        {

            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != id.ToString())
            {
                return Forbid(); 
            }

            var success = await _usuario.UpdateAsync(id, usuarioDto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id:int}/coleta")]
        [Authorize]
        public async Task<IActionResult> GetInformacoesColeta(int id)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != id.ToString())
            {
                return Forbid();
            }

            var info = await _usuario.GetInformacoesColetaAsync(id);
            if (info == null)
            {
                return NotFound("Usuário não encontrado ou sem endereço cadastrado.");
            }
            return Ok(info);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Recebe os dois valores da tupla retornada pelo serviço
            var (usuario, token) = await _usuario.LoginAsync(loginDto);

            if (token == null || usuario == null)
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            usuario.SenhaHash = "********";

            // Retorna objeto JSON completo que o front-end espera
            return Ok(new { token, usuario });
        }
    }
}
