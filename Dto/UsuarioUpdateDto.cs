using System.ComponentModel.DataAnnotations;

namespace HoraDoLixo.Dto
{
    public class UsuarioUpdateDto
    {
        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(255)]
        public string? EnderecoRua { get; set; }

        [StringLength(8)]
        public string? EnderecoNumero { get; set; }

        [StringLength(100)]
        public string? EnderecoComplemento { get; set; }

        [StringLength(100)]
        public string? EnderecoBairro { get; set; }

        [StringLength(8)]
        public string? Cep { get; set; }

        public TimeSpan? HorarioAlertaComum { get; set; }
        public bool? AlertaComumAtivo { get; set; }
        public TimeSpan? HorarioAlertaSeletiva { get; set; }
        public bool? AlertaSeletivaAtivo { get; set; }
    }
}
