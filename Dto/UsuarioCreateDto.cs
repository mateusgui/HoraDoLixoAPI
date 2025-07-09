using System.ComponentModel.DataAnnotations;

namespace HoraDoLixo.Dto
{
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(255, MinimumLength = 3)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(20, MinimumLength = 8)]
        public string Senha { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "A rua é obrigatória.")]
        [StringLength(255)]
        public string EnderecoRua { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(8)]
        public string EnderecoNumero { get; set; }

        [StringLength(100)]
        public string? EnderecoComplemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(100)]
        public string EnderecoBairro { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(8)]
        public string Cep { get; set; }
    }
}
