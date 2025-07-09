namespace HoraDoLixo.Dto
{
    public class ZonaColetaInfoDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public List<ProgramacaoDto> Programacao { get; set; } = new();
    }
}
