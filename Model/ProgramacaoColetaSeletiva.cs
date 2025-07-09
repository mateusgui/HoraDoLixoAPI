using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HoraDoLixo.Model
{
    public class ProgramacaoColetaSeletiva
    {
        /// <summary>
        /// Chave primária da programação de coleta. Coluna de identidade.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_coleta_seletiva")]
        public int IdColetaSeletiva { get; set; }

        /// <summary>
        /// Chave estrangeira para a zona de coleta seletiva.
        /// </summary>
        [Required]
        [Column("id_zona_coleta_seletiva")]
        public int IdZonaColetaSeletiva { get; set; }

        /// <summary>
        /// Dia da semana em que a coleta ocorre.
        /// (Recomenda-se usar uma convenção, ex: 1 para Domingo, 2 para Segunda, etc.,
        /// ou mapear para um Enum DayOfWeek).
        /// </summary>
        [Required]
        [Column("dia_semana")]
        public int DiaSemana { get; set; }

        /// <summary>
        /// Horário previsto para o início da coleta.
        /// </summary>
        [Required]
        [Column("horario_inicio_previsto")]
        public TimeSpan HorarioInicioPrevisto { get; set; }

        /// <summary>
        /// Observações adicionais sobre a programação da coleta.
        /// </summary>
        [StringLength(255)]
        [Column("observacoes")]
        public string? Observacoes { get; set; }

        // Propriedade de Navegação (Chave Estrangeira)

        /// <summary>
        /// Propriedade de navegação para a Zona de Coleta Seletiva associada.
        /// </summary>
        [ForeignKey("IdZonaColetaSeletiva")]
        public virtual ZonaColetaSeletiva ZonaColetaSeletiva { get; set; }
    }
}
