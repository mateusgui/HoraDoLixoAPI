using HoraDoLixo.Model;

namespace HoraDoLixo.ServiceInterface
{
    public interface IProgramacaoColetaSeletivaService
    {
        IEnumerable<ProgramacaoColetaSeletiva> GetAll();
    }
}
