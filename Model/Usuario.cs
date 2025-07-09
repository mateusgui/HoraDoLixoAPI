using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HoraDoLixo.Model
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(255)]
        [Column("nome_completo")]
        public string NomeCompleto { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("senha_hash")]
        public string SenhaHash { get; set; }

        [StringLength(20)]
        [Column("telefone")]
        public string? Telefone { get; set; }

        [StringLength(255)]
        [Column("endereco_rua")]
        public string? EnderecoRua { get; set; }

        [StringLength(8)]
        [Column("endereco_numero")]
        public string? EnderecoNumero { get; set; }

        [StringLength(100)]
        [Column("endereco_complemento")]
        public string? EnderecoComplemento { get; set; }

        [StringLength(100)]
        [Column("endereco_bairro")]
        public string? EnderecoBairro { get; set; }

        [StringLength(8)]
        [Column("cep", TypeName = "char(8)")]
        public string? Cep { get; set; }

        [Column("latitude", TypeName = "decimal(9, 6)")]
        public decimal? Latitude { get; set; }

        [Column("longitude", TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }

        [Required]
        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; }

        [Required]
        [Column("status")]
        public bool Status { get; set; }

        [Column("horario_alerta_comum")]
        public TimeSpan? HorarioAlertaComum { get; set; }

        [Required]
        [Column("alerta_comum_ativo")]
        public bool AlertaComumAtivo { get; set; }

        [Column("horario_alerta_seletiva")]
        public TimeSpan? HorarioAlertaSeletiva { get; set; }

        [Required]
        [Column("alerta_seletiva_ativo")]
        public bool AlertaSeletivaAtivo { get; set; }
    }
}
