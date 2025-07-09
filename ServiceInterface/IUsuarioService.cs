using HoraDoLixo.Dto;
using HoraDoLixo.Model;

namespace HoraDoLixo.ServiceInterface
{
    public interface IUsuarioService
    {
        IEnumerable<Usuario> GetAll();
        Usuario? GetById(int id);
        Task<Usuario> CreateAsync(UsuarioCreateDto usuarioDto);
        Task<(Usuario? usuario, string? token)> LoginAsync(LoginDto loginDto); //
        Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto); 
        Task<InformacoesColetaDto?> GetInformacoesColetaAsync(int id);
    }
}
