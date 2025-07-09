namespace HoraDoLixo.Dto
{
    public class ProgramacaoDto
    {
        public int DiaSemana { get; set; }
        public TimeSpan HorarioInicioPrevisto { get; set; }
        public string? Observacoes { get; set; }
    }
}
